using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Folio
{
    public class YouTubeCrawler : ICrawler
    {
        private readonly YouTubeSource source;

        public YouTubeCrawler(YouTubeSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            this.source = source;
        }

        public string Description
        {
            get
            {
                return source.Comment;
            }
        }

        public IEnumerable<Resource> Execute()
        {
            Trace.WriteLine("Starting " + source.PlaylistId);
            return CrawlPlaylist(source.PlaylistId);
        }

        private IEnumerable<Resource> CrawlPlaylist(string playlistId)
        {
            return
                from videoIdChunk in GetPlaylistVideoIds(playlistId)
                from videoChunk in GetVideo(videoIdChunk, "snippet,recordingDetails,contentDetails")
                from video in videoChunk
                select new YouTubeResource
                {
                    Id = video.Id,
                    PrimaryLanguage = source.PrimaryLanguage,
                    PrimaryLanguageType = source.PrimaryLanguageType,
                    SecondaryLanguage = source.SecondaryLanguage,
                    SecondaryLanguageType = source.SecondaryLanguageType,
                    Title = video.Snippet.Title,
                    Description = video.Snippet.Description,
                    Channel = video.Snippet.ChannelId,
                    RecordingDate = video.RecordingDetails != null ? video.RecordingDetails.RecordingDate : null,
                    RecordingPlace = video.RecordingDetails != null ? video.RecordingDetails.LocationDescription : null,
                    Duration = video.ContentDetails.Duration,
                };
        }

        static YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = "AIzaSyAAr3G3-rT2IvTky57BjWmmBBNfEp_hHls",
            ApplicationName = "Folio"
        });

        static IEnumerable<IEnumerable<string>> GetPlaylistVideoIds(string playlistId)
        {
            var request = youtubeService.PlaylistItems.List("contentDetails");
            request.PlaylistId = playlistId;
            request.MaxResults = 50;
            do
            {
                var response = request.Execute();
                yield return
                    from pli in response.Items
                    select pli.ContentDetails.VideoId;

                request.PageToken = response.NextPageToken;
            } while (request.PageToken != null);
        }

        static string GetUploadsPlaylistId(string channelId = null, string username = null)
        {
            var request = youtubeService.Channels.List("contentDetails");
            request.Id = channelId;
            request.ForUsername = username;
            var response = request.Execute();
            return response.Items.Single().ContentDetails.RelatedPlaylists.Uploads;
        }

        static IEnumerable<IEnumerable<Video>> GetVideo(IEnumerable<string> ids, string parts)
        {
            var request = youtubeService.Videos.List(parts);
            request.Id = String.Join(",", ids);
            do
            {
                var response = request.Execute();
                yield return response.Items;
                request.PageToken = response.NextPageToken;
            } while (request.PageToken != null);
        }
    }
}

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
                select new Resource
                {
                    Source = source.SourceId,
                    Type = RecordType.Video,
                    Id = video.Id,
                    Url = new Uri(String.Format("https://www.youtube.com/watch?v={0}", video.Id)),
                    PrimaryLanguage = source.PrimaryLanguage,
                    PrimaryLanguageType = source.PrimaryLanguageType,
                    SecondaryLanguage = source.SecondaryLanguage,
                    SecondaryLanguageType = source.SecondaryLanguageType,
                    Title = video.Snippet.Title,
                    Description = video.Snippet.Description,
                    DateTags = GetDateTags(video).Distinct().ToArray(),
                };
        }

        static readonly DateParser dateParser = new DateParser();
        private static IEnumerable<string> GetDateTags(Video video)
        {
            if (video.Snippet != null)
                foreach (var tag in dateParser.GetDateTags(video.Snippet.Title + video.Snippet.Description))
                    yield return tag;

            if (video.RecordingDetails != null && video.RecordingDetails.RecordingDate.HasValue)
                yield return video.RecordingDetails.RecordingDate.Value.ToString("yyyyMMdd");
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

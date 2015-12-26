using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Folio
{
    public class YouTubeRecordSource : RecordSource
    {
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
        public LanguageType PrimaryLanguageType { get; set; }
        public LanguageType SecondaryLanguageType { get; set; }

        private readonly string playlistId;
        private readonly string name;

        public YouTubeRecordSource(string playlistId, string name)
        {
            this.playlistId = playlistId;
            this.name = name;
        }

        protected string SourceId
        {
            get
            {
                return string.Format("https://www.youtube.com/playlist?list={0}", playlistId);
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override IEnumerable<Resource> FetchAll()
        {
            return
                from videoIdChunk in GetPlaylistVideoIds(playlistId)
                from videoChunk in GetVideo(videoIdChunk, "snippet,recordingDetails,contentDetails")
                from video in videoChunk
                select new Resource
                {
                    Source = SourceId,
                    Type = RecordType.Video,
                    Id = video.Id,
                    Url = new Uri(String.Format("https://www.youtube.com/watch?v={0}", video.Id)),
                    PrimaryLanguage = PrimaryLanguage,
                    PrimaryLanguageType = PrimaryLanguageType,
                    SecondaryLanguage = SecondaryLanguage,
                    SecondaryLanguageType = SecondaryLanguageType,
                    Title = video.Snippet.Title,
                    Description = video.Snippet.Description,
                    DateTags = GetDateTags(video).Distinct().ToArray(),
                };
        }

        private static IEnumerable<DateTag> GetDateTags(Video video)
        {
            if (video.Snippet != null)
                foreach (var tag in DateTag.Find(video.Snippet.Title + video.Snippet.Description))
                    yield return tag;

            if (video.RecordingDetails != null && video.RecordingDetails.RecordingDate.HasValue)
                yield return DateTag.FromDate(video.RecordingDetails.RecordingDate.Value);
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

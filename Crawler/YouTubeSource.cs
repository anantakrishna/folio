﻿using System.Collections.Generic;
namespace Folio
{
    public class YouTubeSource
    {
        public string PlaylistId { get; set; }
        public string Comment { get; set; }
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
        public LanguageType PrimaryLanguageType { get; set; }
        public LanguageType SecondaryLanguageType { get; set; }

        public static IEnumerable<YouTubeSource> All
        {
            get
            {
                yield return new YouTubeSource
                {
                    PlaylistId = "UUwJj2mAHaA3Q9ETTCiscSzA",
                    Comment = "Rasaraja dasa",
                    PrimaryLanguage = "ENG",
                    PrimaryLanguageType = LanguageType.Original,
                };
                
                yield return new YouTubeSource
                {
                    PlaylistId = "PLBC9A211D7D1B70F3",
                    Comment = "Vraja Mandal Parikrama 1989 w/ Srila Gurudeva, Eng. subs",
                    PrimaryLanguage = "HIN",
                    PrimaryLanguageType = LanguageType.Original,
                    SecondaryLanguage = "ENG",
                    SecondaryLanguageType = LanguageType.Subtitles,
                };
                yield return new YouTubeSource
                {
                    PlaylistId = "PURLxpoBLZs5QDCUcmQlijHA",
                    Comment = "isadas",
                };

                yield return new YouTubeSource
                {
                    PlaylistId = "UUJuJel0s6o0G1GTL5-i3-gw",
                    Comment = "Taralaksa (Srila Gurudev channel)",
                    PrimaryLanguage = "ENG",
                    PrimaryLanguageType = LanguageType.Original,
                    SecondaryLanguage = "RUS",
                    SecondaryLanguageType = LanguageType.Voiceover,
                };
                
            }
        }
    }
}
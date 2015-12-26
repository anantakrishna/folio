using System.Collections.Generic;
using System.Linq;

namespace Folio
{
    public static class RecordSourceList
    {
        public static IEnumerable<RecordSource> All
        {
            get
            {
                yield return new SbnmcdRecordSource();
                yield return new PurebhaktiComRecordSource();
                yield return new PurebhaktiRuRecordSource();

                yield return new YouTubeRecordSource("UUwJj2mAHaA3Q9ETTCiscSzA", "rasaraja")
                {
                    PrimaryLanguage = "ENG",
                    PrimaryLanguageType = LanguageType.Original,
                };

                yield return new YouTubeRecordSource("PLBC9A211D7D1B70F3", "vraja-mandala-eng-sub")
                {
                    PrimaryLanguage = "HIN",
                    PrimaryLanguageType = LanguageType.Original,
                    SecondaryLanguage = "ENG",
                    SecondaryLanguageType = LanguageType.Subtitles,
                };

                yield return new YouTubeRecordSource("UURLxpoBLZs5QDCUcmQlijHA", "PureBhakti.tv")
                {
                };

                yield return new YouTubeRecordSource("UUJuJel0s6o0G1GTL5-i3-gw", "taralaksa")
                {
                    PrimaryLanguage = "ENG",
                    PrimaryLanguageType = LanguageType.Original,
                    SecondaryLanguage = "RUS",
                    SecondaryLanguageType = LanguageType.Voiceover,
                };
                yield return new YouTubeRecordSource("PL67AF8198D0E0CBEF", "krsna-karunya-vraja-mandala")
                {
                };
            }
        }

        public static RecordSource GetByName(string name)
        {
            return (
                from source in All
                where source.Name == name
                select source
                ).SingleOrDefault();
        }
    }
}

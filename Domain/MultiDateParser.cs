using Folio.Parsing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Folio
{
    public class MultiDateParser : IDateTagExtractor
    {
        private static readonly IEnumerable<IDateTagExtractor> extractors;

        static IEnumerable<IDateTagExtractor> CreateExtractors()
        {
            yield return new SingleDateExtractor(@"\d{8}", @"yyyyMMdd");
            yield return new SingleDateExtractor(@"\d{4}([-/\\]\d{1,2}){2}", @"yyyy/MM/dd", @"yyyy\\MM\\dd", @"yyyy-MM-dd");
            yield return new SingleDateExtractor(@"\d{4} " + MonthPattern + @" \d{1,2}", @"yyyy MMM d");
            /// 05JUN16
            yield return new SingleDateExtractor(@"\d{2}" + MonthPattern + @"\d{2}", @"yyMMMd");
            /// 07_March_26
            yield return new SingleDateExtractor(@"\d{2}_" + MonthPattern + @"_\d{2}", @"yy_MMMM_d");
            yield return new SingleDateExtractor(MonthPattern + @" \d{1,2},? \d{4}", @"MMM d yyyy", @"MMM d, yyyy", @"MMMM d, yyyy");
            yield return new SingleDateExtractor(MonthPattern + @" \d{1,2} '?\d{2}", @"MMM d, \'yy", @"MMM d, yy");
            yield return new SingleDateExtractor(MonthPattern + @" \d{1,2}-\d{4}", CultureInfo.GetCultureInfo("ru-RU"), @"MMMM d-yyyy");
            //yield return new SingleDateExtractor(@"\d{1,2} " + MonthPattern + @"[\.,]? (\d{4}|'?\d{2})", );
            //yield return new SingleDateExtractor(@"", );

        }

        private const string EnglishMonthPattern = @"(Jan(uary)?|Feb(ruary)?|Mar(ch)?|Apr(il)?|May|June?|July?|Aug(ust)?|Sep(t(ember)?)?|Oct(ober)?|Nov(ember)?|Dec(ember)?)";
        private const string RussianMonthPattern = @"(Янв(арь)?|Фев(раль)?|Март?|Апр(ель)?|Май|Июнь|Июль|Авг(уст)?|Сен(т(ябрь)?)?|Окт(ябрь)?|Ноя(брь)?|Дек(абрь)?)";
        private const string MonthPattern = "(" + EnglishMonthPattern + "|" + RussianMonthPattern + ")";

        static MultiDateParser()
        {
            extractors = CreateExtractors();
        }

        public IEnumerable<string> GetDateTags(string input)
        {
            return
                from extractor in extractors
                from tag in extractor.GetDateTags(input)
                select tag;
        }
    }
}

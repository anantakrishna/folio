using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Folio
{
    public class DateParser
    {
        public IEnumerable<string> GetDateTags(string text)
        {
            return
                from rawTag in FindDateTags(text)
                from tag in Normalize(rawTag)
                select tag;
        }

        public IEnumerable<string> FindDateTags(string input)
        {
            return
                from match in dateTagRegex.Matches(input).OfType<Match>()
                select match.Value;
        }

        public IEnumerable<string> Normalize(string rawTag)
        {
            DateTime result;
            foreach (var culture in cultures)
                if (DateTime.TryParseExact(rawTag, formats, culture, DateTimeStyles.AllowWhiteSpaces, out result))
                {
                    yield return result.ToString("yyyyMMdd");
                    yield break;
                }
        }

        private readonly string[] formats = {
            @"yyyyMMdd",
            @"yyyyMMd",
            @"yyyy/MM/dd",
            @"yy/MM/dd",
            @"yyyy\\MM\\dd",
            @"yyyy-MM-dd",
            @"yyyy MMM d",
            @"yyMMMd",
            @"yy_MMMM_d",
            @"MMddyy",
            @"M/d/yyyy",
            @"M dd yy",
            @"M_dd_yy",
            @"MMM. d, yyyy",
            @"MMM. d yyyy",
            @"MMM d, yyyy",
            @"MMM d, yy",
            @"MMM d, \'yy",
            @"MMM d yyyy",
            @"MMMM d, yyyy",
            @"MMMM d-yyyy",
            @"MMMM d yy",
            @"d-MM-yy",
            @"d MMM. yyyy",
            @"d MMM yyyy",
            @"d MMMM yyyy",
            @"d MMMM, yyyy",
        };

        private const string EnglishMonthPattern = @"(Jan(uary)?|Feb(ruary)?|Mar(ch)?|Apr(il)?|May|June?|July?|Aug(ust)?|Sep(t(ember)?)?|Oct(ober)?|Nov(ember)?|Dec(ember)?)";
        private const string RussianMonthPattern = @"(Янв(арь)?|Фев(раль)?|Март?|Апр(ель)?|Май|Июнь|Июль|Авг(уст)?|Сен(т(ябрь)?)?|Окт(ябрь)?|Ноя(брь)?|Дек(абрь)?)";
        private const string MonthPattern = "(" + EnglishMonthPattern + "|" + RussianMonthPattern + ")";

        private static readonly string[] dateTagPatternsSimplified =
        {
            MonthPattern + @"[\s\p{P}]*\d{1,2}[\s\p{P}]*\d{2,4}",
            @"(\d{1,4}[\s\p{P}]?)?" + MonthPattern + @"([\s\p{P}]?\d{1,4})?",
            @"\d{4,8}([\s\p{P}]\d{1,2})*",
            @"(\d{1,2}[\s\p{P}]){2}\d{2,4}",
        };

        private static readonly string[] dateTagPatterns =
        {
            @"\d{6,8}", /// yyyyMMdd, yyMMdd, yyyyMMd
            @"\d{1,2} \d\d \d\d", // MM dd yy
            @"\d{1,2}_\d\d_\d\d", // MM_dd_yy
            @"\d{1,2}[-/\\]\d{1,2}[-/\\]\d{2,4}", // MM/dd/yyyy
            @"(\d{4}|\d{2})[-/\\]\d{1,2}[-/\\]\d{1,2}", // yyyy/MM/dd or yyyy\MM\dd rus

            /// yyyy MMM d
            @"\d{4} " + MonthPattern + @" \d{1,2}",
            @"\d{2}" + MonthPattern + @"\d{2}", // 05JUN16
            @"\d{2}_" + MonthPattern + @"_\d{2}", // 07_March_26
            /// MMM d yyyy
            MonthPattern + @"\.? \d{1,2},? (\d{4}|'?\d{2})",
            MonthPattern + @" \d{1,2}-\d{4}",

            /// d MMM yyyy
            @"\d{1,2} " + MonthPattern + @"[\.,]? (\d{4}|'?\d{2})",
        };

        private static readonly Regex dateTagRegex = new Regex(String.Join("|", dateTagPatterns), RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly CultureInfo[] cultures = {
            CultureInfo.InvariantCulture,
            EnSeptCultureInfo,
            CultureInfo.GetCultureInfo("ru-RU"),
        };

        static CultureInfo EnSeptCultureInfo
        {
            get
            {
                var culture = new CultureInfo("en-US");
                culture.DateTimeFormat.AbbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec", "" };
                return culture;
            }
        }
    }
}

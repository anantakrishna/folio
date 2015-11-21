using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Folio
{
    [System.ComponentModel.TypeConverter(typeof(Converters.DateTagTypeConverter))]
    public struct DateTag : IComparable<DateTag>
    {
        public enum Matching
        {
            Exact,
            Inclusive,
        }

        private readonly DateTime date;
        private readonly string format;

        private DateTag(DateTime date, string format)
        {
            this.date = date;
            this.format = format;
        }

        public DateTag(string tag)
        {
            this.format = DateFormat;
            if (DateTime.TryParseExact(tag, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out this.date))
                return;
            this.format = YearAndMonthFormat;
            if (DateTime.TryParseExact(tag, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out this.date))
                return;
            this.format = YearFormat;
            if (DateTime.TryParseExact(tag, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out this.date))
                return;

            throw new ArgumentException("Cannot parse date tag");
        }

        private const string YearFormat = "yyyy";
        private const string YearAndMonthFormat = YearFormat + "-MM";
        private const string DateFormat = YearAndMonthFormat + "-dd";

        public static DateTag FromYear(int year)
        {
            return new DateTag(new DateTime(year, 1, 1), YearFormat);
        }

        public static DateTag FromYearAndMonth(int year, int month)
        {
            return new DateTag(new DateTime(year, month, 1), YearAndMonthFormat);
        }

        public static DateTag FromDate(int year, int month, int day)
        {
            return FromDate(new DateTime(year, month, day));
        }

        public static DateTag FromDate(DateTime date)
        {
            return new DateTag(date, DateFormat);
        }

        public override string ToString()
        {
            return date.ToString(format);
        }

        public static implicit operator string(DateTag tag)
        {
            return tag.ToString();
        }

        public static implicit operator DateTag(string tag)
        {
            return new DateTag(tag);
        }

        public int CompareTo(DateTag other)
        {
            if (other == null)
                return 1;

            return this.ToString().CompareTo(other.ToString());
        }


        #region Parsing
        public static IEnumerable<DateTag> Find(string text)
        {
            return
                from match in regex.Matches(text).OfType<Match>()
                from tag in Normalize(match.Value)
                select tag;
        }

        private static IEnumerable<DateTag> Normalize(string rawTag)
        {
            DateTime result;
            foreach (var culture in cultures)
                if (DateTime.TryParseExact(rawTag, dateFormats, culture, DateTimeStyles.AllowWhiteSpaces, out result))
                {
                    yield return FromDate(result);
                    yield break;
                }
        }

        private static readonly string[] dateFormats = {
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

        private static readonly string[] patternsSimplified =
        {
            MonthPattern + @"[\s\p{P}]*\d{1,2}[\s\p{P}]*\d{2,4}",
            @"(\d{1,4}[\s\p{P}]?)?" + MonthPattern + @"([\s\p{P}]?\d{1,4})?",
            @"\d{4,8}([\s\p{P}]\d{1,2})*",
            @"(\d{1,2}[\s\p{P}]){2}\d{2,4}",
        };

        private static readonly string[] patterns =
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

        private static readonly Regex regex = new Regex(String.Join("|", patterns), RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly CultureInfo[] cultures = {
            CultureInfo.InvariantCulture,
            EnSeptCultureInfo,
            CultureInfo.GetCultureInfo("ru-RU"),
        };

        private static CultureInfo EnSeptCultureInfo
        {
            get
            {
                var culture = new CultureInfo("en-US");
                culture.DateTimeFormat.AbbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec", "" };
                return culture;
            }
        }
        #endregion
    }
}

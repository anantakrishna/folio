using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cliver;

namespace Folio
{
    public class DateParser
    {
        private readonly string[] formats = {
            @"yyyyMMdd",
            @"yyyy/MM/dd",
            @"yy/MM/dd",
            @"yyyy\\MM\\dd",
            @"yyyy-MM-dd",
            @"MMddyy",
            @"M/d/yyyy",
            @"yyyy MMM d",
            @"MM dd yy",
            @"MMM. d, yyyy",
            @"MMM. d yyyy",
            @"MMM d, yyyy",
            @"MMM d, yy",
            @"MMM d, \'yy",
            @"MMM d yyyy",
            @"MMMM d, yyyy",
            @"MMMM d-yyyy",
            @"d MMM. yyyy",
            @"d MMMM yyyy",
        };

        private const string EnglishMonthPattern = @"(Jan(uary)?|Feb(ruary)?|Mar(ch)?|Apr(il)?|May|June?|July?|Aug(ust)?|Sep(t(ember)?)?|Oct(ober)?|Nov(ember)?|Dec(ember)?)";
        private const string RussianMonthPattern = @"(Янв(арь)?|Фев(раль)?|Март?|Апр(ель)?|Май|Июнь|Июль|Авг(уст)?|Сен(т(ябрь)?)?|Окт(ябрь)?|Ноя(брь)?|Дек(абрь)?)";
        private const string MonthPattern = "(" + EnglishMonthPattern + "|" + RussianMonthPattern + ")";

        private static readonly string[] dateTagPatterns =
        {
            @"\d{8}", // yyyyMMdd
            @"\d{6}", // ddMMyy
            @"\d\d \d\d \d\d", // dd MM yy
            @"\d{1,2}[-/\\]\d{1,2}[-/\\]\d{2,4}", // MM/dd/yyyy
            @"(\d{4}|\d{2})[-/\\]\d{1,2}[-/\\]\d{1,2}", // yyyy/MM/dd or yyyy\MM\dd rus

            // yyyy MMM d
            @"\d{4} " + MonthPattern + @" \d{1,2}",

            // MMM d yyyy
            MonthPattern + @"\.? \d{1,2},? (\d{4}|'?\d{2})",
            MonthPattern + @" \d{1,2}-\d{4}",

            // d MMM yyyy
            @"\d{1,2} " + MonthPattern + @"\.? (\d{4}|'?\d{2})",
        };

        private static readonly Regex dateTagPattern;
        private static readonly CultureInfo[] cultures = {
            CultureInfo.InvariantCulture,
            new CultureInfo("en-US"), //later modified
            CultureInfo.GetCultureInfo("ru-RU"),
        };

        static DateParser()
        {
            dateTagPattern = new Regex(String.Join("|", dateTagPatterns), RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            cultures[1].DateTimeFormat.AbbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec", "" };
        }

        public string GetDateTag(string text)
        {
            var dt = GetDate(text);
            if (dt.HasValue)
                return dt.Value.ToString("yyyyMMdd");

            return null;
        }

        private DateTime? GetDate(string text)
        {
            var match = dateTagPattern.Match(text);
            if (!match.Success)
                return null;

            DateTime result;
            foreach (var culture in cultures)
                if (DateTime.TryParseExact(match.Value, formats, culture, DateTimeStyles.AllowWhiteSpaces, out result))
                    return result;
            return null;
        }

        private DateTime? GetDate2(string text)
        {
            DateTime result;
            if (DateTimeRoutines.TryParseDate(text, DateTimeRoutines.DateTimeFormat.USA_DATE, out result))
                return result;
            return null;
        }
    }
}

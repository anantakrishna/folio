using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Folio.Parsing
{
    public class IntervalDateExtractor : IDateTagExtractor
    {
        private readonly Regex regex = new Regex(@"(?<main>\d{4}/\d{2}/\d{2})-(?<second>\d{2})", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly string[] formats = {
            @"yyyy/MM/dd",
        };

        public IEnumerable<string> GetDateTags(string input)
        {
            var match = regex.Match(input);
            if (!match.Success)
                yield break;

            var main = match.Groups["main"].Value;
            var second = int.Parse(match.Groups["second"].Value);

            DateTime date;
            if (!DateTime.TryParseExact(main, formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date))
                yield break;

            while (date.Day <= second)
            {
                yield return date.ToString("yyyyMMdd");
                date = date.AddDays(1);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Folio.Parsing
{
    public class SingleDateExtractor : IDateTagExtractor
    {
        public IEnumerable<string> GetDateTags(string text)
        {
            var match = regex.Match(text);
            if (!match.Success)
                yield break;

            DateTime result;
            if (DateTime.TryParseExact(match.Value, formats, culture, DateTimeStyles.AllowWhiteSpaces, out result))
                yield return result.ToString("yyyyMMdd");
        }

        private readonly Regex regex;
        private readonly string[] formats;
        private readonly CultureInfo culture;

        public SingleDateExtractor(string pattern, params string[] formats)
            : this(pattern, CultureInfo.InvariantCulture, formats)
        {

        }

        public SingleDateExtractor(string pattern, CultureInfo culture, params string[] formats)
        {
            this.regex = new Regex(pattern, RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            this.formats = formats;
            this.culture = culture;
        }
    }
}

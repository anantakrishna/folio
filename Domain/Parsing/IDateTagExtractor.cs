using System.Collections.Generic;

namespace Folio.Parsing
{
    public interface IDateTagExtractor
    {
        IEnumerable<string> GetDateTags(string input);
    }
}
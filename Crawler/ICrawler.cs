using System.Collections.Generic;

namespace Folio
{
    public interface ICrawler
    {
        string Description { get; }
        IEnumerable<Resource> Execute();
    }
}

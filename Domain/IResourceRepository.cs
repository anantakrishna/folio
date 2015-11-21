using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio
{
    public interface IResourceRepository
    {
        void Add(IEnumerable<Resource> records);
        IQueryable<Resource> Query { get; }
        IQueryable<Resource> ByDateTag(DateTag tag, DateTag.Matching matching);
    }
}

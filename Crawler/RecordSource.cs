using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio
{
    public abstract class RecordSource
    {
        public abstract string Name { get; }
        public abstract IEnumerable<Resource> FetchAll();
    }
}

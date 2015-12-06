using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio
{
    public static partial class Extensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) where T : class
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}

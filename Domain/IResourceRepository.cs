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
    }

    public static partial class Extensions
    {
        public static IQueryable<Resource> ByYear(this IResourceRepository repository, int year)
        {
            return
                from resource in repository.Query
                where resource.DateTags.Any(dt => dt.StartsWith(year.ToString()))
                select resource;
        }

        public static IQueryable<Resource> ByYearAndMonth(this IResourceRepository repository, int year, int month)
        {
            return
                from resource in repository.Query
                where resource.DateTags.Any(dt => dt.StartsWith(string.Format("{0}{1}", year, month)))
                select resource;
        }

        public static IQueryable<Resource> ByDate(this IResourceRepository repository, DateTime date)
        {
            return
                from resource in repository.Query
                where resource.DateTags.Contains(date.ToString("yyyyMMdd"))
                select resource;
        }
    }
}

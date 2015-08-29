using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Folio
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public async static Task Crawl(TextWriter log
            , [Table("Sources")] IQueryable<CrawlerSource> sources
            , [Table("Resources")] ICollector<Resource> resources
            )
        {
            var tasks =
                from source in sources.ToList()
                from crawler in GetCrawlers(source.URL)
                where crawler != null
                select crawler.ExecuteAsync(r => resources.Add(r));

            await Task.WhenAll(tasks);
        }

        public static void Test([QueueTrigger("test")] string message, TextWriter log)
        {
            log.WriteLine("Message: {0}", message);
        }
    }
}

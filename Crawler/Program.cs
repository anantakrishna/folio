using Folio.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Folio
{
    public class Program
    {
        static void Main()
        {
            Crawl();
            Console.WriteLine("Finished crawling");
            Console.ReadKey();

        }

        private static void Crawl()
        {
            var records =
                from crawler in Crawlers
                from record in crawler.Execute()
                select record;

            using (var repository = new RecordRepository())
            {
                repository.Add(records);
            }
        }

        private static IEnumerable<ICrawler> Crawlers
        {
            get
            {
                yield return new SbnmcdCrawler();
                foreach (var youtubeSource in YouTubeSource.All)
                    yield return new YouTubeCrawler(youtubeSource);
            }
        }
    }
}

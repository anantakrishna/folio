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
            //WindowsAzure.Table.EntityConverters.TypeData.EntityTypeMap.RegisterAssembly(typeof(Program).Assembly);
            Crawl();
            Console.WriteLine("Finished crawling");

            //Parse();
            Console.WriteLine("Finished parsing");
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

        private static void Parse()
        {
            var parser = new DateParser();
            var records =
                from line in System.IO.File.ReadAllLines("records.txt")
                select new
                {
                    Line = line,
                    DateTags = parser.GetDateTags(line)
                };

            var notFound =
                from record in records
                where !record.DateTags.Any()
                select record.Line;

            Console.WriteLine("Not found: {0}", notFound.Count());
            System.IO.File.WriteAllLines("unknown.txt", notFound);

            var found = 
                from record in records
                where record.DateTags.Any()
                select record.Line;
            Console.WriteLine("Detected: {0}", found.Count());
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

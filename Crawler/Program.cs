using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsAzure.Table;

namespace Folio
{
    public class Program
    {
        static void Main()
        {
            //WindowsAzure.Table.EntityConverters.TypeData.EntityTypeMap.RegisterAssembly(typeof(Program).Assembly);
            //Crawl();
            Console.WriteLine("Finished crawling");

            Parse();
            Console.WriteLine("Finished parsing");
            Console.ReadKey();

        }

        private static void Crawl()
        {
            var records =
                from crawler in Crawlers
                from record in crawler.Execute()
                select record.ToString();

            System.IO.File.WriteAllLines("records.txt", records);
        }

        private static void Parse()
        {
            var parser = new DateParser();
            var records =
                from line in System.IO.File.ReadAllLines("records.txt")
                    //where parser.GetDateTag(line) == null
                select new
                {
                    Line = line,
                    DateTag = parser.GetDateTag(line)
                };

            var notFound =
                from record in records
                where record.DateTag == null
                select record.Line;

            Console.WriteLine("Not found: {0}", notFound.Count());
            System.IO.File.WriteAllLines("unknown.txt", notFound);

            var found = 
                from record in records
                where record.DateTag != null
                select record.Line;
            Console.WriteLine("Detected: {0}", found.Count());

            foreach (var record in records)
            {
                if (record.DateTag == null)
                    continue;

                if (record.DateTag == "19970526")
                    Console.WriteLine(record.Line);
            }
        }

        private static IEnumerable<ICrawler> Crawlers
        {
            get
            {
                foreach (var youtubeSource in YouTubeSource.All)
                    yield return new YouTubeCrawler(youtubeSource);
            }
        }
    }
}

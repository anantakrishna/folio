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
            Parse();
            Console.ReadKey();

        }

        private static void Crawl()
        {
            var records =
                from crawler in Crawlers
                from record in crawler.Execute()
                select record.ToString();

            System.IO.File.AppendAllLines("records.txt", records);
        }

        private static void Parse()
        {
            var parser = new DateParser();
            var lines =
                from line in System.IO.File.ReadAllLines("records.txt")
                where parser.GetDateTag(line) == null
                select line;

            Console.WriteLine("Not found: {0}", lines.Count());
            System.IO.File.WriteAllLines("unknown.txt", lines);
            lines =
                from line in System.IO.File.ReadAllLines("records.txt")
                where parser.GetDateTag(line) != null
                select line;
            Console.WriteLine("Detected: {0}", lines.Count());
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

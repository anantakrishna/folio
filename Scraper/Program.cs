using HtmlAgilityPack;
using Nest;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio.Scraper
{
    public static class Program
    {
        private static ElasticClient Client { get; set; }
        private const string RawRecordIndexName = "raw";

        public static void Main(string[] args)
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            // Step 3. Set target properties 
            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(rule1);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            Client = new ElasticClient(ConnectionSettings);
            Index();
        }

        private static IConnectionSettingsValues ConnectionSettings
        {
            get
            {
                return new ConnectionSettings(new Uri("http://localhost:9200"))
                    .InferMappingFor<Record>(x => x.IndexName(RawRecordIndexName))
                    ;
            }
        }

        static void Index()
        {
            var web = new HtmlWeb();

            var records = new PurebhaktiComSpider(web).Scrape();

            var result = Client.IndexMany(records);

            if (!result.IsValid)
            {
                foreach (var item in result.ItemsWithErrors)
                    Trace.WriteLine(string.Format("Failed to index document {0}: {1}", item.Id, item.Error));
            }
        }
    }
}

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Folio.Scraping
{
    abstract internal class Spider
    {
        private readonly HtmlWeb web = new HtmlWeb();

        private readonly Queue<Uri> urlQueue = new Queue<Uri>();
        protected Queue<Uri> Urls { get { return urlQueue; } }

        public void Run()
        {
            while (Urls.Any())
            {
                var url = Urls.Dequeue();
                var document = web.Load(url.AbsoluteUri);
                ProcessListPage(document);
            }
        }

        protected virtual void ProcessListPage(HtmlDocument document)
        {
            var nextUrl = ExtractNextUrl(document);
            if (nextUrl != null)
                Urls.Enqueue(nextUrl);

            foreach (var itemUrl in ExtractItemUrls(document))
            {
                var itemDocument = web.Load(itemUrl.AbsoluteUri);
                ProcessItem(itemDocument);
            }
        }

        protected abstract Uri ExtractNextUrl(HtmlDocument document);
        protected abstract IEnumerable<Uri> ExtractItemUrls(HtmlDocument document);
        protected abstract void ProcessItem(HtmlDocument document);
    }
}
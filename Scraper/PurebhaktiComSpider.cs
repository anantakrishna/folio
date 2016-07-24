using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HapCss;
using System.Globalization;
using NLog;

namespace Folio.Scraper
{
    class PurebhaktiComSpider
    {
        private static readonly Uri sitemapUrl = new Uri(@"http://www.purebhakti.com/resources/sitemap.html");
        private readonly HtmlWeb web;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public PurebhaktiComSpider(HtmlWeb web)
        {
            this.web = web;
        }

        public IEnumerable<Record> Scrape()
        {
            var sitemap = web.Load(sitemapUrl.AbsoluteUri);

            var links = sitemap.DocumentNode.QuerySelectorAll("ul.level_3 a[href^='/teachers/bhakti-discourses-mainmenu-61/']");

            return
                from link in links
                where link.Attributes["href"] != null
                let url = new Uri(sitemapUrl, link.Attributes["href"].Value)
                let record = GetRecord(url)
                where record != null
                select record;
        }

        private ArticleRecord GetRecord(Uri url)
        {
            logger.Trace(url.AbsoluteUri);
            var document = web.Load(url.AbsoluteUri);
            var article = document.DocumentNode.QuerySelector("article");

            if (article == null)
                return null;

            var date = ExtractPublicationDate(article);
            article.QuerySelector("ul.actions").Remove();
            article.QuerySelector("dl.article-info").Remove();
            article.QuerySelector("ul.pager").Remove();

            return new ArticleRecord
            {
                Url = url,
                Title = article.QuerySelector("h2").InnerText.Trim(),
                Text = article.OuterHtml,
                PublicationDate = date,
            };
        }

        private static DateTime? ExtractPublicationDate(HtmlNode article)
        {
            var dateNode = article.QuerySelector("dl.article-info > dd.create");
            if (dateNode == null)
                return null;

            DateTime date;
            if (DateTime.TryParse(dateNode.InnerText.Trim(), CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out date))
                return date;

            return null;
        }
    }
}

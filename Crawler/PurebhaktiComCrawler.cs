using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Folio
{
    public class PurebhaktiComCrawler : ICrawler
    {
        private static readonly Uri sitemapUrl = new Uri(@"http://www.purebhakti.com/resources/sitemap.html");

        private const string SourceName = @"purebhakti.com";
        public string Description
        {
            get
            {
                return "purebhakti.com";
            }
        }

        public IEnumerable<Resource> Execute()
        {
            var doc = GetDocument(sitemapUrl);

            var links = doc.DocumentNode.SelectNodes("//div[@id='xmap']//li/a[@title='Bhakti Discourses']/../ul/li/ul/li/a");

            return
                from link in links.EmptyIfNull()
                where link.Attributes["href"] != null
                let url = new Uri(sitemapUrl, link.Attributes["href"].Value)
                let resource = GetResource(url)
                where resource != null
                select resource;
        }

        private static readonly Regex itemUrlRegex = new Regex(@"\/(\d+)-[^/]*\.html$");

        private Resource GetResource(Uri url)
        {
            Trace.WriteLine(url.AbsolutePath);
            var match = itemUrlRegex.Match(url.AbsolutePath);
            if (!match.Success)
                return null;

            var id = match.Groups[1].Value;
            var document = GetDocument(url);
            var article = document.DocumentNode.SelectSingleNode("//article");

            if (article == null)
                return null;

            article.SelectSingleNode("ul[@class='actions']").Remove();
            return new Resource
            {
                Id = id,
                Url = url,
                Title = ExtractTitle(article),
                Type = RecordType.Text,
                Source = SourceName,
                Text = article.InnerText,
                DateTags = ExtractDateTags(article),
            };
        }

        private static string ExtractTitle(HtmlNode article)
        {
            var titleNode = article.SelectSingleNode("h2");
            if (titleNode == null)
                return null;
            return titleNode.InnerText.Trim();
        }

        private static string ExtractText(HtmlNode article)
        {
            var paragraphs =
                from p in article.SelectNodes("p")
                select p.InnerText.Trim();
            return String.Join("\n", paragraphs.ToArray());
        }

        private static IEnumerable<DateTag> ExtractDateTags(HtmlNode article)
        {
            var dateNode = article.SelectSingleNode("dl[@class='article-info']/dd[@class='create']");
            if (dateNode == null)
                yield break;

            DateTime date;
            if (DateTime.TryParse(dateNode.InnerText.Trim(), CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out date))
                yield return DateTag.FromDate(date);
        }

        private static HtmlDocument GetDocument(Uri url)
        {
            HttpClient http = new HttpClient();
            var response = http.GetByteArrayAsync(url);
            response.Wait();
            var result = response.Result;
            var source = WebUtility.HtmlDecode(Encoding.UTF8.GetString(result, 0, result.Length - 1));
            var doc = new HtmlDocument();
            doc.LoadHtml(source);
            return doc;
        }
    }
}

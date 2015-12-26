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
    public class PurebhaktiRuRecordSource : RecordSource
    {
        private static readonly Uri rootpUrl = new Uri(@"http://purebhakti.ru/index.php?option=com_content&view=category&id=5&Itemid=4&limit=0");

        public override string Name
        {
            get
            {
                return "purebhakti.ru";
            }
        }

        public override IEnumerable<Resource> FetchAll()
        {
            var doc = GetDocument(rootpUrl);

            var links = doc.DocumentNode.SelectNodes("//div[@class='bodycontent']//table[@class='category']//tr/td[2]/a");

            return
                from link in links.EmptyIfNull()
                where link.Attributes["href"] != null
                let url = new Uri(rootpUrl, link.Attributes["href"].Value)
                let resource = GetResource(url)
                where resource != null
                select resource;
        }

        private static readonly Regex itemUrlRegex = new Regex(@"\bid=(\d+)");

        private Resource GetResource(Uri url)
        {
            Trace.WriteLine(url.PathAndQuery);
            var match = itemUrlRegex.Match(url.Query);
            if (!match.Success)
                return null;

            var id = match.Groups[1].Value;
            var document = GetDocument(url);
            var article = document.DocumentNode.SelectSingleNode("//div[@class='full-article']");

            if (article == null)
                return null;

            article.SelectSingleNode("//div[@class='socbuttons']").Remove();
            return new Resource
            {
                Id = id,
                Url = url,
                Title = ExtractTitle(article),
                Type = RecordType.Text,
                Source = Name,
                Text = ExtractText(article),
                DateTags = ExtractDateTags(article),
            };
        }

        private static string ExtractTitle(HtmlNode article)
        {
            var titleNode = article.SelectSingleNode("//h2[@class='contentheading']");
            if (titleNode == null)
                return null;
            return titleNode.InnerText.Trim();
        }

        private static string ExtractText(HtmlNode article)
        {
            return article.InnerText;
        }

        private static IEnumerable<DateTag> ExtractDateTags(HtmlNode article)
        {
            return DateTag.Find(article.InnerText);
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

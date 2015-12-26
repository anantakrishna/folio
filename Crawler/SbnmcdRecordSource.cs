using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Folio
{
    public class SbnmcdRecordSource : RecordSource
    {
        public override string Name
        {
            get
            {
                return "sbnmcd.org";
            }
        }

        private readonly Uri rootUrl = new Uri("http://sbnmcd.org/all_mp3/");

        public override IEnumerable<Resource> FetchAll()
        {
            HttpClient http = new HttpClient();
            var response = http.GetByteArrayAsync(rootUrl);
            response.Wait();
            var result = response.Result;
            var source = WebUtility.HtmlDecode(Encoding.UTF8.GetString(result, 0, result.Length - 1));
            var doc = new HtmlDocument();
            doc.LoadHtml(source);

            return
                from link in doc.DocumentNode.SelectNodes("//tr[@class='file_bg1']//a")
                let url = new Uri(rootUrl, link.Attributes["href"].Value)
                select new Resource
                {
                    Id = url.AbsoluteUri,
                    Url = url,
                    Title = link.InnerText,
                    Type = RecordType.Audio,
                    Source = Name,
                    DateTags = DateTag.Find(link.InnerText),
                };
        }
    }
}

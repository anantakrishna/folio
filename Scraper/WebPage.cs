using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio.Scraping
{
    [ElasticsearchType(IdProperty = "Url")]
    class WebPage
    {
        [String]
        public Uri Url { get; set; }
        public string Html { get; set; }
    }
}

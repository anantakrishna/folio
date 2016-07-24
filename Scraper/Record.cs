using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio.Scraper
{
    [ElasticsearchType(IdProperty = "Url")]
    class Record
    {
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}

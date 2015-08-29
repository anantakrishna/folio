using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio
{
    public class YouTubeResource : Resource
    {
        public override string Source
        {
            get
            {
                return "YouTube";
            }
            set
            {
            }
        }
        public string Channel { get; set; }
        public DateTime? RecordingDate { get; set; }
        public string RecordingPlace { get; set; }
        public string Duration { get; set; }

        public override Uri Url
        {
            get { return new Uri(String.Format("https://www.youtube.com/watch?v={0}", Id)); }
            set { }
        }

        public override RecordType Type
        {
            get { return RecordType.Video; }
            set { }
        }
    }
}

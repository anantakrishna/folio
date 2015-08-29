using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio
{
    public enum TagType
    {
        Date,
        Language,
        Occasion,
        Place,
        Form,
        Keyword,
    }

    public enum TagOrigin
    {
        Automatic,
        Manual,
    }

    public class Tag
    {
        public TagType Type { get; set; }
        public string Value { get; set; }
        public TagOrigin Origin { get; set; }
        public string Author { get; set; }
    }
}

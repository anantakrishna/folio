using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folio
{
    public class Resource
    {
        public virtual string Source { get; set; }
        public virtual string Id { get; set; }
        public virtual Uri Url { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Text { get; set; }
        public virtual RecordType Type { get; set; }

        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
        public LanguageType PrimaryLanguageType { get; set; }
        public LanguageType SecondaryLanguageType { get; set; }

        public IEnumerable<DateTag> DateTags { get; set; }
    }

}

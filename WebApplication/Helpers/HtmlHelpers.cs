using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Folio.Web
{
    public static class HtmlHelpers
    {
        private static readonly IDictionary<RecordType, string> recordTypeMap = new Dictionary<RecordType, string> {
            { RecordType.Unknown, "file-o" },
            { RecordType.Text, "file-text-o" },
            { RecordType.Audio, "file-audio-o" },
            { RecordType.Video, "file-video-o" },
        };

        public static string RecordTypeIcon(this HtmlHelper helper, RecordType recordType)
        {
            return recordTypeMap[recordType];
        }
    }
}
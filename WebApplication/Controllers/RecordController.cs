using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Folio.Web.Controllers
{
    public class RecordController : Controller
    {
        private readonly IResourceRepository repository;

        public RecordController(IResourceRepository repository)
        {
            this.repository = repository;
        }

        // GET: Record
        [Route("records/{year:int}-{month:int}-{day:int}")]
        public ActionResult Index(int year, int month, int day)
        {
            var tag = DateTag.FromDate(year, month, day);
            var records = repository.ByDateTag(tag, DateTag.Matching.Exact).ToArray();
            return View("RecordList", records);
        }
    }
}
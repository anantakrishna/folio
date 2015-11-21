using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Folio.Web.Controllers
{
    [RoutePrefix("cal")]
    public class CalendarController : Controller
    {
        private readonly IResourceRepository repository;

        public CalendarController(IResourceRepository repository)
        {
            this.repository = repository;
        }

        [Route()]
        public ActionResult Years()
        {
            var items =
                from year in Enumerable.Range(1980, 2010 - 1980 + 1)
                select new CalendarItem
                {
                    Url = Url.Action("Months", new { Year = year }),
                    Label = year.ToString(),
                    RecordCount = repository.ByDateTag(DateTag.FromYear(year), DateTag.Matching.Inclusive).Count(),
                };
            ViewBag.Title = Resources.CalendarTitle;
            return View("List", items);
        }

        [Route("{year:int}")]
        public ViewResult Months(int year)
        {
            var items =
                from month in Enumerable.Range(1, 12)
                select new CalendarItem
                {
                    Url = Url.Action("Days", new { Year = year, Month = month }),
                    Label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    RecordCount = repository.ByDateTag(DateTag.FromYearAndMonth(year, month), DateTag.Matching.Inclusive).Count(),
                };
            ViewBag.Title = year.ToString();
            ViewBag.Breadcrumbs = new MenuItem[]
            {
                new MenuItem { Url = Url.Action("Years"), Label = Resources.CalendarTitle },
            };
            return View("List", items);
        }

        [Route("{year:int}-{month:int}")]
        public ViewResult Days(int year, int month)
        {
            var items =
                from day in Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                select new CalendarItem
                {
                    Url = Url.Action("Index", "Records", new { Year = year, Month = month, Day = day }),
                    Label = day.ToString(),
                    RecordCount = repository.ByDateTag(DateTag.FromDate(year, month, day), DateTag.Matching.Inclusive).Count(),
                };
            ViewBag.Title = string.Format("{0}, {1}", year, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));
            ViewBag.Breadcrumbs = new MenuItem[]
            {
                new MenuItem { Url = Url.Action("Years"), Label = Resources.CalendarTitle },
                new MenuItem { Url = Url.Action("Months", new { Year = year }), Label = year.ToString() },
            };
            return View("List", items);
        }
    }
}
using System;
using System.Linq;
using System.Web.Mvc;

namespace Folio.Web.Controllers
{
    [RoutePrefix("crawler")]
    [Route("{action=index}")]
    public class CrawlerController : Controller
    {
        private readonly IResourceRepository repository;

        public CrawlerController(IResourceRepository repository)
        {
            this.repository = repository;
        }

        // GET: Crwaler
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        [Route("preview/{name}")]
        public ActionResult Preview(string name)
        {
            var source = RecordSourceList.GetByName(name);
            var resources = source.FetchAll();
            return View("Resources", resources);
        }

        [HttpPost]
        public ActionResult Crawl()
        {
            var records = (
                from source in RecordSourceList.All
                from record in source.FetchAll()
                select record
                ).ToArray();

            repository.Add(records);
            TempData["SuccessMessage"] = String.Format(Resources.CrawlingFinished, records.Count());
            return RedirectToAction("Index");
        }
    }
}
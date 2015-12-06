using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult PureBhaktiCom()
        {
            var crawler = new PurebhaktiComCrawler();
            var resources = crawler.Execute();
            return View("Resources", resources);
        }

        [HttpPost]
        public ActionResult Crawl()
        {
            var records = (
                from crawler in Crawlers
                from record in crawler.Execute()
                select record
                ).ToArray();

            repository.Add(records);
            TempData["SuccessMessage"] = String.Format(Resources.CrawlingFinished, records.Count());
            return RedirectToAction("Index");
        }

        private static IEnumerable<ICrawler> Crawlers
        {
            get
            {
                yield return new PurebhaktiComCrawler();
                yield return new SbnmcdCrawler();
                foreach (var youtubeSource in YouTubeSource.All)
                    yield return new YouTubeCrawler(youtubeSource);
            }
        }
    }
}
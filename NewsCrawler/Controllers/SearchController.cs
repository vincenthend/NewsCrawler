using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsCrawler.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(String searchQuery, int searchType)
        {
            ViewBag.searchCount = 5;
            ViewBag.searchQuery = searchQuery;
            ViewBag.searchType = searchType;
            return View();
        }
    }
}
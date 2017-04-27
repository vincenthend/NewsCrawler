using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsCrawler.Models;

namespace NewsCrawler.Controllers
{
    public class SearchController : Controller
    {
        private NewsDB dbNews = new NewsDB();

        public ActionResult Index(String searchQuery, int searchType)
        {
            MetadataDB dbMeta = new MetadataDB();
            //Check meta, update if last update > 1 hours

            News[] newsArray = dbNews.News.ToArray();


            

            ViewBag.searchQuery = searchQuery;
            ViewBag.searchType = searchType;
            return View();
        }

        public News[] searchKMP(News[] newsList)
        {
            int i;
            for (i = 0; i < newsList.Length; i++)
            {
                //Search Title

                //Search Description
            }

            return null;
        }
    }
}
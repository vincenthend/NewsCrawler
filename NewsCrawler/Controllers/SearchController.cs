using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsCrawler.Models;
using NewsCrawler.Controllers;

namespace NewsCrawler.Controllers {
  public class SearchController : Controller {
    //private NewsDB dbNews = new NewsDB();

    /**
    * Mencari string pada newsList dengan urutan judul baru kemudian isi.
    * 
    * @param newsList list berita dari database
    * @param searchQuery search pattern yang digunakan
    * @return List berita yang lolos search dengan lokasi ditemukannya
    */
    public ActionResult Index(string searchQuery, int searchType) {

      // Check meta, update if last update > 1 hours
      //MetadataDB dbMeta = new MetadataDB();

      // Convert news list to array
      //News[] newsArray = dbNews.News.ToArray();
      //List<NewsFound> newsFound = new List<NewsFound>();

      // Do Search
      /*if (searchType == 0) {
        newsFound = searchKMP(newsArray, searchQuery);
      }
      else if (searchType == 1) {
        newsFound = searchBM(newsArray, searchQuery);
      }
      else if (searchType == 2) {
        //Regex
      }
      else if (searchType == 3) {
        //debug to show all news
        newsFound = showAll(newsArray);
      }*/

      ViewBag.regexTry = Searcher.regexConvert(searchQuery);
      ViewBag.searchQuery = searchQuery;
      ViewBag.searchType = searchType;
      //ViewBag.searchResult = newsFound;
      //ViewBag.searchCount = newsFound.Count;
      
      return View();
    }

    /**
     * Mencari string pada newsList dengan algoritma KMP judul baru kemudian isi.
     * 
     * @param newsList list berita dari database
     * @param searchQuery search pattern yang digunakan
     * @return List berita yang lolos search dengan lokasi ditemukannya
     */
    private List<NewsFound> searchKMP(News[] newsList, string searchQuery) {
      int i;
      int foundLoc;
      List<NewsFound> foundList = new List<NewsFound>();

      // Begin searching in all newsList
      for (i = 0; i < newsList.Length; i++) {
        // Search Title
        foundLoc = Searcher.KMPSearchFirst(newsList[i].title, searchQuery);
        if (foundLoc == -1) {
          // Search Description
          Searcher.KMPSearchFirst(newsList[i].content, searchQuery);
          if (foundLoc != -1) {
            // Found in description
            foundList.Add(new NewsFound(newsList[i], foundLoc, false));
          }
        }
        else {
          // Found in title
          foundList.Add(new NewsFound(newsList[i], foundLoc, true));
        }
      }

      return foundList;
    }

    /**
     * Mencari string pada newsList dengan algoritma BM judul baru kemudian isi.
     * 
     * @param newsList list berita dari database
     * @param searchQuery search pattern yang digunakan
     * @return List berita yang lolos search dengan lokasi ditemukannya
     */
    private List<NewsFound> searchBM(News[] newsList, string searchQuery) {
      int i;
      int foundLoc;
      List<NewsFound> foundList = new List<NewsFound>();

      // Begin searching in all newsList
      for (i = 0; i < newsList.Length; i++) {
        // Search Title
        foundLoc = Searcher.BMSearchFirst(newsList[i].title, searchQuery);
        if (foundLoc == -1) {
          // Search Description
          Searcher.BMSearchFirst(newsList[i].content, searchQuery);
          if (foundLoc != -1) {
            // Found in description
            foundList.Add(new NewsFound(newsList[i], foundLoc, false));
          }
        }
        else {
          // Found in Title
          foundList.Add(new NewsFound(newsList[i], foundLoc, true));
        }
      }

      return foundList;
    }

    /**
     * Menampilkan newsList
     * 
     * @param newsList list berita dari database
     * @return List semua berita
     */
    private List<NewsFound> showAll(News[] newsList) {
      int i;
      int foundLoc = 0;
      List<NewsFound> foundList = new List<NewsFound>();

      // Begin searching in all newsList
      for (i = 0; i < newsList.Length; i++) {
        { 
          foundList.Add(new NewsFound(newsList[i], foundLoc, true));
        }
      }

      return foundList;
    }
  }
}
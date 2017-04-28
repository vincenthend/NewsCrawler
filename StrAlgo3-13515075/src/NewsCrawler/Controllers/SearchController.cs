using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Web.Mvc;
using NewsCrawler.Models;
using NewsCrawler.Controllers;
using MySql.Data.Entity;
using System.Data.Entity;

namespace NewsCrawler.Controllers {
  public class SearchController : Controller {

    /**
    * Mencari string pada newsList dengan urutan judul baru kemudian isi.
    * 
    * @param newsList list berita dari database
    * @param searchQuery search pattern yang digunakan
    * @return List berita yang lolos search dengan lokasi ditemukannya
    */
    public ActionResult Index(string searchQuery, int searchType) {
      DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
      String connectionString = "server=127.0.0.1;User Id=root;password=password;database=db_newscrawler";
      MySqlConnection connection = new MySqlConnection(connectionString);

      NewsCrawlerDB dbNews = new NewsCrawlerDB(connection, false);
      dbNews.Database.CreateIfNotExists();

      connection.Open();

      // Empty tables
      MySqlCommand cmd = new MySqlCommand("delete from News", connection);
      cmd.ExecuteNonQuery();
      dbNews.SaveChanges();

      MySqlTransaction transaction = connection.BeginTransaction();
      dbNews.Database.UseTransaction(transaction);
      // Get data from RSS
      Loader.loadRSS("http://rss.detik.com/index.php/detikcom", dbNews);
      Loader.loadRSS("http://tempo.co/rss/terkini", dbNews);
      Loader.loadRSS("http://rss.vivanews.com/get/all", dbNews);
      transaction.Commit();

      // Convert news list to array
      News[] newsArray = dbNews.News.SqlQuery("select * from News").ToArray();
      List<NewsFound> newsFound = new List<NewsFound>();

      // Do Search
      if (searchType == 0) {
        newsFound = searchKMP(newsArray, searchQuery);
      }
      else if (searchType == 1) {
        newsFound = searchBM(newsArray, searchQuery);
      }
      else if (searchType == 2) {
        newsFound = searchRegex(newsArray, searchQuery);
      }
      else if (searchType == 3) {
        //debug to show all news
        newsFound = showAll(newsArray);
      }

      ViewBag.regexQuery = Searcher.regexConvert(searchQuery);
      ViewBag.searchQuery = searchQuery;
      ViewBag.searchType = searchType;
      ViewBag.searchResult = newsFound;
      ViewBag.searchCount = newsFound.Count;

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
            if (newsList[i].content.Length >= 50 + foundLoc) {
              newsList[i].content = newsList[i].content.Substring(foundLoc, 49) + "...";
            }
            else {
              newsList[i].content = newsList[i].content.Substring(foundLoc);
            }

            foundList.Add(new NewsFound(newsList[i], foundLoc, false));
          }
        }
        else {
          // Found in title
          if (newsList[i].content.Length >= 50) {
            newsList[i].content = newsList[i].content.Substring(0, 49) + "...";
          }

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
            if (newsList[i].content.Length >= 50 + foundLoc) {
              newsList[i].content = newsList[i].content.Substring(foundLoc, 49) + "...";
            }
            else {
              newsList[i].content = newsList[i].content.Substring(foundLoc);
            }
            foundList.Add(new NewsFound(newsList[i], foundLoc, false));
          }
        }
        else {
          // Found in Title
          if (newsList[i].content.Length >= 50) {
            newsList[i].content = newsList[i].content.Substring(0, 49) + "...";
          }
          foundList.Add(new NewsFound(newsList[i], foundLoc, true));
        }
      }

      return foundList;
    }

    /**
     * Mencari string pada newsList dengan regex judul baru kemudian isi.
     * 
     * @param newsList list berita dari database
     * @param searchQuery search pattern yang digunakan
     * @return List berita yang lolos search dengan lokasi ditemukannya
     */
    private List<NewsFound> searchRegex(News[] newsList, string searchQuery) {
      int i;
      int foundLoc;
      List<NewsFound> foundList = new List<NewsFound>();

      // Begin searching in all newsList
      for (i = 0; i < newsList.Length; i++) {
        // Search Title
        foundLoc = Searcher.RegexSearchFirst(newsList[i].title, searchQuery);
        if (foundLoc == -1) {
          // Search Description
          Searcher.RegexSearchFirst(newsList[i].content, searchQuery);
          if (foundLoc != -1) {
            // Found in description
            foundList.Add(new NewsFound(newsList[i], foundLoc, false));
            if (newsList[i].content.Length >= 50 + foundLoc) {
              newsList[i].content = newsList[i].content.Substring(foundLoc, 49) + "...";
            }
            else {
              newsList[i].content = newsList[i].content.Substring(foundLoc);
            }
          }
        }
        else {
          // Found in Title
          foundList.Add(new NewsFound(newsList[i], foundLoc, true));
          if (newsList[i].content.Length >= 50) {
            newsList[i].content = newsList[i].content.Substring(0, 49) + "...";
          }
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
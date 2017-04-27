using MySql.Data.MySqlClient;
using NewsCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace NewsCrawler.Controllers {
  public class Loader {
    public static void loadRSS(string url, NewsCrawlerDB db, MySqlConnection connection) {
      //Create the XmlDocument.
      XmlDocument doc = new XmlDocument();
      doc.Load(url);

      XmlNodeList linkList = doc.GetElementsByTagName("guid");
      XmlNodeList titleList = doc.GetElementsByTagName("title");
      XmlNodeList contentList = doc.GetElementsByTagName("description");
      XmlNodeList dateList = doc.GetElementsByTagName("pubDate");

      MySqlTransaction transaction = connection.BeginTransaction();
      try {
        db.Database.UseTransaction(transaction);

        List<News> news = new List<News>();
        int titleoff = titleList.Count - linkList.Count;
        int contentoff = contentList.Count - linkList.Count;
        for (int i = 0; i < linkList.Count; i++) {
          news.Add(new News { url = linkList[i].InnerXml, title = titleList[i + titleoff].InnerXml, content = contentList[i + contentoff].InnerXml, date = dateList[i].InnerXml });
        }

        db.News.AddRange(news);
        db.SaveChanges();


        transaction.Commit();
      }
      catch {
        transaction.Rollback();
      }
    }

  }
}
using MySql.Data.MySqlClient;
using NewsCrawler.Models;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using HtmlAgilityPack;
using System.Linq;

namespace NewsCrawler.Controllers {
  public class Loader {
    public static void loadRSS(string url, NewsCrawlerDB db, MySqlConnection connection, string contentPlaceholder) {
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

        string[] content = new string[contentList.Count - contentoff];

        HtmlWeb web;
        HtmlDocument docs;

        for (int i = 0; i < linkList.Count; i++) {
          web = new HtmlWeb();
          docs = web.Load(linkList[i].InnerXml);
          HtmlNode[] findclasses = docs.DocumentNode.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes.Contains(contentPlaceholder)).ToArray();

          news.Add(new News { url = linkList[i].InnerXml, title = titleList[i + titleoff].InnerXml, content = findclasses[0].InnerText, date = dateList[i].InnerXml });
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
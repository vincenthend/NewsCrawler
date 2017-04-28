using MySql.Data.MySqlClient;
using NewsCrawler.Models;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using HtmlAgilityPack;
using System.Linq;
using System.Text;
using System;
using System.Data.Entity.Validation;
using Microsoft.Ajax.Utilities;

namespace NewsCrawler.Controllers {
  public class Loader {
    public static void loadRSS(string url, NewsCrawlerDB db) {
      //Create the XmlDocument.
      XmlDocument doc = new XmlDocument();
      doc.Load(url);

      XmlNodeList linkList = doc.GetElementsByTagName("guid");
      XmlNodeList titleList = doc.GetElementsByTagName("title");
      XmlNodeList contentList = doc.GetElementsByTagName("description");
      XmlNodeList dateList = doc.GetElementsByTagName("pubDate");


      List<News> news = new List<News>();
      int titleoff = titleList.Count - linkList.Count;
      int contentoff = contentList.Count - linkList.Count;

      string[] content = new string[contentList.Count - contentoff];

      for (int i = 0; i < linkList.Count; i++) {
        WebClient client = new WebClient();
        string htmlText = null;

        htmlText = client.DownloadString(linkList[i].InnerText);

        if (htmlText != null) {
          HtmlDocument htmlDoc = new HtmlDocument();
          htmlDoc.LoadHtml(htmlText);
          var nodes = htmlDoc.DocumentNode.SelectNodes("//p");
          StringBuilder sb = new StringBuilder();
          if (nodes != null) {
            foreach (var item in nodes) {
              string text = item.OuterHtml;
              if (!string.IsNullOrEmpty(text)) {
                sb.AppendLine(text.Trim());
              }
            }
          }
          content[i] = sb.ToString();
        }

        news.Add(new News { url = linkList[i].InnerXml, title = titleList[i + titleoff].InnerXml, content = content[i], date = dateList[i].InnerXml });
        db.SaveChanges();
      }
      db.News.AddRange(news);
      db.SaveChanges();


    }
  }
}
using System.Net;

namespace ConsoleApplication3
{
    using System;
    using System.Xml;
    using System.Windows;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;
    public class Sample
    {
        public static void  getRSS(string xmlText)
        {
            //Create the XmlDocument.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlText);

            //Display all the book titles.
            XmlNodeList titleList = xmlDoc.GetElementsByTagName("title");
            XmlNodeList urlList = xmlDoc.GetElementsByTagName("link");
            XmlNodeList dateList = xmlDoc.GetElementsByTagName("pubDate");
            string[] title = new string[dateList.Count];
            string[] url = new string[dateList.Count];
            string[] date = new string[dateList.Count];
            string[] content = new string[dateList.Count];
            int corrector = 2;
            string distinct;
            if (titleList[0].ToString() == "news.detik")
            {
                corrector = 2;
                distinct = "//div[@class='detail_text'][@id='detikdetailtext']";
            }
            else if (titleList[0].ToString() == "Tempo.co News Site")
            {
                corrector = 2;
                distinct = "//p";
            }
            else if (titleList[0].ToString() == "VIVA.co.id")
            {
                corrector = 2;
                distinct = "//span[@itemprop='description']";
            }
            for (int i = 0; i < titleList.Count - corrector; i++)
            {
                date[i] = dateList[i].InnerXml;
                title[i] = titleList[i + corrector].InnerXml;
                url[i] = urlList[i + corrector].InnerXml;
                WebClient client = new WebClient();
                string htmlText = null;
                try
                {
                    htmlText = client.DownloadString(url[i]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error page not loaded");
                }
                if (htmlText != null)
                {
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(htmlText);
                    var nodes = htmlDoc.DocumentNode.SelectNodes("//p");
                    StringBuilder sb = new StringBuilder();
                    if (nodes != null)
                    {
                        foreach (var item in nodes)
                        {
                            string text = item.OuterHtml;
                            if (!string.IsNullOrEmpty(text))
                            {
                                sb.AppendLine(text.Trim());
                            }
                            Console.WriteLine();
                        }
                    }
                    content[i] = sb.ToString();
                    Console.WriteLine(content[i]);
                    Console.Read();
                }
            }
            Console.Read();
        }
        public static void Main(string[] args)
        {
            getRSS("detikcom.xml");
        }
    }
}

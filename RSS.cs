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
<<<<<<< HEAD
            string[] title = new string[titleLi st.Count];
            string[] url = new string[urlList.Count];
=======
            string[] title = new string[dateList.Count];
            string[] url = new string[dateList.Count];
>>>>>>> a6588620b2e0626e302fd5e57ba35e901926ec0f
            string[] date = new string[dateList.Count];
            string[] content = new string[dateList.Count];
            for (int i = 0; i < dateList.Count; i++)
            {
                date[i] = dateList[i].InnerXml;
                title[i] = titleList[i + 2].InnerXml;
                url[i] = urlList[i + 2].InnerXml;
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

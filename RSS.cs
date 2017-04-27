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
            string[] title = new string[titleList.Count];
            string[] url = new string[urlList.Count];
            string[] date = new string[dateList.Count];
            string[] content = new string[dateList.Count];
            date[0] = dateList[0].InnerXml;
            for (int i = 1; i < urlList.Count; i++)
            {
                title[i] = titleList[i].InnerXml;
                url[i] = urlList[i].InnerXml;
                WebClient client = new WebClient();
                string htmlText = client.DownloadString(url[i]);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(htmlText);
                StringBuilder sb = new StringBuilder();
                IEnumerable<HtmlNode> nodes = htmlDoc.DocumentNode.Descendants().Where(n =>
                   n.NodeType == HtmlNodeType.Text &&
                   n.ParentNode.Name != "script" &&
                   n.ParentNode.Name != "style");
                foreach (HtmlNode node in nodes)
                {
                    Console.WriteLine(node.InnerText);
                    //content[i] = htmlDoc.GetElementsByTagName("p");
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

using System.Net;

namespace ConsoleApplication3
{
    using System;
    using System.Xml;
    using System.Windows.Forms;

    public class Sample
    {
        public static string[] getRSS(string xmlText)
        {
            //Create the XmlDocument.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlText);

            //Display all the book titles.
            XmlNodeList titleList = xmlDoc.GetElementsByTagName("title");
            XmlNodeList urlList = xmlDoc.GetElementsByTagName("link");
            XmlNodeList dateList = xmlDoc.GetElementsByTagName("pubDate");
            string[] title = new string[titleLi st.Count];
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
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.load(htmlText);
                content[i] = htmlDoc.GetElementsByTagName("p");
            }
            Console.Read();
        }
    }
}

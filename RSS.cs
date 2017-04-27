using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    using System;
    using System.IO;
    using System.Xml;

    public class Sample
    {
        public static string[] getRSS(string text)
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(text);

            //Display all the book titles.
            XmlNodeList elemList = doc.GetElementsByTagName("link");
            string[] s = new string[elemList.Count];
            for (int i = 0; i < elemList.Count; i++)
            {
                s[i] = elemList[i].InnerXml;
                Console.WriteLine(s[i]);
            }
            Console.Read();
            return s;
        }
    }
}

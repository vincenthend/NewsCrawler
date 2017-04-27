using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class KMP
    {
        public static int[] KMPStringMatching(string text, string pattern)
        {
            int n;
            int m;
            int i;
            int j;
            int[] fail;
            List<int> idxList;
            text = text.ToLower();
            pattern = pattern.ToLower();
            n = text.Length;
            m = pattern.Length;
            i = 0;
            j = 0;
            fail = computeFail(pattern);
            idxList = new List<int>();
            while (i < n)
            {
                Console.WriteLine(" i = " + i);
                Console.WriteLine(" j = " + j);
                if (text[i] == pattern[j])
                {
                    Console.WriteLine("Masuk 1");
                    if (j == m - 1)
                    {
                        Console.WriteLine("Cek" + (i - m + 1));
                        idxList.Add(i - m + 1);
                    }
                    i++;
                    j++;
                    Console.WriteLine("Cek j1 " + j);
                    if (j >= m)
                    {
                        j = fail[j - 1];
                    }
                    Console.WriteLine("Cek j2 " + j);
                }
                else if (j > 0)
                {
                    Console.WriteLine("Masuk 2");
                    j = fail[j - 1];
                }
                else
                {
                    Console.WriteLine("Masuk 3");
                    i++;
                }
            }
            return idxList.ToArray();
        }
        public static int[] computeFail(string pattern)
        {
            int[] fail;
            int m;
            int i;
            int j;
            fail = new int[pattern.Length];
            fail[0] = 0;
            i = 1;
            j = 0;
            m = pattern.Length;
            while (i < m)
            {
                if (pattern[i] == pattern[j])
                {
                    fail[i] = j + 1;
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = fail[j - 1];
                }
                else
                {
                    fail[i] = 0;
                    i++;
                }
            }
            return fail;
        }
        public static void Main(string[] args)
        {
            string pattern;
            string text;
            int[] pos;
            while (true)
            {
                pattern = Console.ReadLine();
                text = Console.ReadLine();
                pos = KMPStringMatching(text, pattern);
                for (int i = 0; i < pos.Length; i++)
                {
                    Console.WriteLine(pos[i]);
                }
                Console.ReadLine();
            }
        }
    }
}

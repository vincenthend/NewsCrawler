using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace NewsCrawler.Controllers {
  public class Searcher {
    public static int KMPSearchFirst(string text, string pattern) {
      int n;
      int m;
      int i;
      int j;
      int[] fail;
      text = text.ToLower();
      pattern = pattern.ToLower();
      n = text.Length;
      m = pattern.Length;
      i = 0;
      j = 0;
      fail = computeFail(pattern);
      while (i < n) {
        if (text[i] == pattern[j]) {
          if (j == m - 1) {
            return (i - m + 1);
          }
          i++;
          j++;
          if (j >= m) {
            j = fail[j - 1];
          }
        }
        else if (j > 0) {
          j = fail[j - 1];
        }
        else {
          i++;
        }
      }
      return -1;
    }

    public static int[] KMPSearchAll(string text, string pattern) {
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
      while (i < n) {
        if (text[i] == pattern[j]) {
          if (j == m - 1) {
            idxList.Add(i - m + 1);
          }
          i++;
          j++;
          if (j >= m) {
            j = fail[j - 1];
          }
        }
        else if (j > 0) {
          j = fail[j - 1];
        }
        else {
          i++;
        }
      }
      return idxList.ToArray();
    }

    public static int BMSearchFirst(string text, string pattern) {
      List<int> retVal = new List<int>();
      int m = pattern.Length;
      int n = text.Length;

      int[] badChar = new int[256];

      BadCharHeuristic(pattern, m, ref badChar);

      int s = 0;
      while (s <= (n - m)) {
        int j = m - 1;

        while (j >= 0 && pattern[j] == text[s + j])
          --j;

        if (j < 0) {
          return s;
        }
        else {
          s += Math.Max(1, j - badChar[text[s + j]]);
        }
      }

      return -1;
    }

    public static int[] BMSearchAll(string text, string pattern) {
      List<int> retVal = new List<int>();
      int m = pattern.Length;
      int n = text.Length;

      int[] badChar = new int[256];

      BadCharHeuristic(pattern, m, ref badChar);

      int s = 0;
      while (s <= (n - m)) {
        int j = m - 1;

        while (j >= 0 && pattern[j] == text[s + j])
          --j;

        if (j < 0) {
          retVal.Add(s);
          s += (s + m < n) ? m - badChar[text[s + m]] : 1;
        }
        else {
          s += Math.Max(1, j - badChar[text[s + j]]);
        }
      }

      return retVal.ToArray();
    }

    public static int RegexSearchFirst(string text, string pattern) {
      string regexPattern = regexConvert(pattern);
      Regex regex = new Regex(regexPattern);

      Match matchFound = regex.Match(text);

      if (matchFound.Success) {
        return matchFound.Index;
      }
      else {
        return -1;
      }
    }
    
    private static void BadCharHeuristic(string text, int size, ref int[] badChar) {
      int i;

      for (i = 0; i < 256; i++)
        badChar[i] = -1;

      for (i = 0; i < size; i++)
        badChar[(int)text[i]] = i;
    }

    private static int[] computeFail(string pattern) {
      int[] fail;
      int m;
      int i;
      int j;
      fail = new int[pattern.Length];
      fail[0] = 0;
      i = 1;
      j = 0;
      m = pattern.Length;
      while (i < m) {
        if (pattern[i] == pattern[j]) {
          fail[i] = j + 1;
          i++;
          j++;
        }
        else if (j > 0) {
          j = fail[j - 1];
        }
        else {
          fail[i] = 0;
          i++;
        }
      }
      return fail;
    }

    public static string regexConvert(string text) {
      string[] regexList;
      string tempString = "";
      string regexConcat = "";

      //Split per word
      regexList = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      // Create regex
      int i;
      int j;
      for (i = 0; i < regexList.Length; i++) {
        if (regexList[i].Length > 3) {
          regexList[i] = regexList[i];
        }
        else {
          for (j = 0; j < regexList[i].Length; j++) {
            tempString = tempString + regexList[i].ElementAt(j) + "\\w* *";
          }
          regexList[i] = tempString;
        }
      }

      //Combine regex
      regexConcat = regexList[0];
      for (i = 1; i < regexList.Length; i++) {
        regexConcat = "(?i:"+regexConcat + "|" + regexList[i] + ")";
      }

      return regexConcat;
    }

  }
}
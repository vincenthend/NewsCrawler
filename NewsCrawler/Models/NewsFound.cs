using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsCrawler.Models {
  public class NewsFound {
    public News newsData { get; set; }
    public int foundLoc { get; set; }
    public bool foundAtTitle { get; set; }

    public NewsFound(News n, int loc, bool foundTitle) {
      newsData = n;
      foundLoc = loc;
      foundAtTitle = foundTitle;
    }

  }
}
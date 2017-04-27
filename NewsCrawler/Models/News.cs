using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NewsCrawler.Models
{
    public class News
    {
        public String ID { get; set; }
        public String title { get; set; }
        public String link { get; set; }
        public String date { get; set; }
        public String image { get; set; }
    }

    public class NewsDB : DbContext
    {
        
    }
}
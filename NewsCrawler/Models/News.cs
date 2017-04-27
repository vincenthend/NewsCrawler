using System;
using System.Data.Entity;

namespace NewsCrawler.Models
{
    public class News
    {
        public String title { get; set; }
        public String url { get; set; }
        public String date { get; set; }
        public String image { get; set; }
        public String content { get; set; }
    }

    public class NewsDB : DbContext
    {
        public DbSet<News> News { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NewsCrawler.Models
{
    public class Metadata
    {
        public DateTime lastUpdate { get; set; }
    }

    public class MetadataDB : DbContext
    {
        public DbSet<Metadata> Metadata { get; set; }
    }
}
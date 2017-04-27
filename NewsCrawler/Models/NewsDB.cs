using System;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.ModelConfiguration.Conventions;
using MySql.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsCrawler.Models {
  [Table("News")]
  public class News {
    [Key]
    [Column("url")]
    public String url { get; set; }
    public String title { get; set; }    
    public String date { get; set; }
    public String content { get; set; }
  }

  [Table("Metadata")]
  public class Metadata {
    [Key]
    [Column("lastUpdate")]
    public DateTime lastUpdate { get; set; }
  }

  [Table("RSSData")]
  public class RSSData {
    [Key]
    [Column("rssName")]
    public String rssName { get; set; }
    public String url { get; set; }    
    public String contentPlaceholder { get; set; }
  }

  [DbConfigurationType(typeof(MySqlEFConfiguration))]
  public class NewsCrawlerDB : DbContext {
    public DbSet<News> News { get; set; }
    public DbSet<RSSData> rssDB { get; set; }
    public DbSet<Metadata> Metadata { get; set; }

    public NewsCrawlerDB(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection){
      
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<News>().MapToStoredProcedures();
      modelBuilder.Entity<RSSData>().MapToStoredProcedures();
      modelBuilder.Entity<Metadata>().MapToStoredProcedures();
    }
  }
}
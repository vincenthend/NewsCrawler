using System.Data.Entity;

namespace NewsCrawler.Models {
  public class DbInitializer : CreateDatabaseIfNotExists<NewsCrawlerDB> {
    protected override void Seed(NewsCrawlerDB context) {
      
    }
  }
}
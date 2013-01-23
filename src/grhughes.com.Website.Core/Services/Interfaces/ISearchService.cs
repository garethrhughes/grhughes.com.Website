namespace grhughes.com.Website.Core.Services.Interfaces
{
  using System.Collections.Generic;
  using Model;

  public interface ISearchService
  {
    void ReIndex();
    SearchResults Search(string query, int page = 0, int limit = 10);
    void Add(BlogPost blogPost);
    void Delete(BlogPost blogPost);
    void Update(BlogPost blogPost);
  }
}
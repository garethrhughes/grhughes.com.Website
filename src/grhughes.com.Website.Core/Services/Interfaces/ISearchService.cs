namespace grhughes.com.Website.Core.Services.Interfaces
{
  using System.Collections.Generic;
  using Model;

  public interface ISearchService<T>
  {
    void Index(IEnumerable<T> data);
    void Index(T data);
    void Delete(T data);
    SearchResults<T> Search(string query, int page = 0, int limit = 10);
  }
}
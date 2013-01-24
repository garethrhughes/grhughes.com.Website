namespace grhughes.com.Website.Core.Model
{
  using System.Collections.Generic;

  public class SearchResults<T>
  {
    public int Page { get; set; }
    public string Query { get; set; }
    public int TotalResults { get; set; }
    public List<T> Results { get; set; }
    public int Limit { get; set; }
  }
}
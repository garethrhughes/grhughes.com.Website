namespace grhughes.com.Website.Core.Model
{
  using System.Collections.Generic;

  public class SearchResults
  {
    public int Page { get; set; }
    public int TotalResults { get; set; }
    public List<BlogPost> Results { get; set; }
    public int ResultsPerPage { get; set; }
  }
}
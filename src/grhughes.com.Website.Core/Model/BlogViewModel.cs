namespace grhughes.com.Website.Core.Model
{
  using System.Collections.Generic;

  public class BlogViewModel
  {
    public int Page { get; set; }
    public int TotalPosts { get; set; }
    public int Limit { get; set; }
    public IList<BlogPost> Posts { get; set; }
  }
}
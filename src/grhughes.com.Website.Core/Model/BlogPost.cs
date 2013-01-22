namespace grhughes.com.Website.Core.Model
{
  using System;

  public class BlogPost
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public DateTime PublishDate { get; set; }

    public DateTime CreationDate { get; set; }

    public bool Published { get; set; }

    public User User { get; set; }

    public string Content { get; set; }
  }
}
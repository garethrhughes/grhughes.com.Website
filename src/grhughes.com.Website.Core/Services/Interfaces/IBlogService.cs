namespace grhughes.com.Website.Core.Services.Interfaces
{
  using System.Collections.Generic;
  using Model;

  public interface IBlogService
  {
    IList<BlogPost> Load(int page, int limit = 10);

    IList<BlogPost> LoadAll();

    BlogPost LoadById(int blogId);

    BlogPost Load(int year, int month, int date, string slug);

    IList<BlogPost> LoadForUser(string email);

    BlogPost Save(BlogPost blogPost);

    void Delete(int blogId);

    void ToggleApproval(int blogId);

    int Count();
  }
}
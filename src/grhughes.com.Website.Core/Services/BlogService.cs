namespace grhughes.com.Website.Core.Services
{
  using System;
  using System.Collections.Generic;
  using Interfaces;
  using Model;

  public class BlogService : BaseService, IBlogService
  {
    private readonly ISearchService<BlogPost> blogSearchService;

    public BlogService(ISearchService<BlogPost> blogSearchService)
    {
      this.blogSearchService = blogSearchService;
    }

    public void Delete(int blogId)
    {
      var db = GetDatabase();

      db.BlogPosts.Delete(db.BlogPosts.Id == blogId);
    }

    public IList<BlogPost> Load(int page, int limit)
    {
      var db = GetDatabase();

      return
        db.BlogPosts.FindAll(db.BlogPosts.Published == true).OrderByPublishDateDescending().Skip(limit*page).Take(limit)
          .ToList<BlogPost>();
    }

    public IList<BlogPost> LoadAll()
    {
      var db = GetDatabase();

      return db.BlogPosts.All().OrderByPublishDateDescending().ToList<BlogPost>();
    }

    public IList<BlogPost> LoadForUser(string email)
    {
      var db = GetDatabase();

      return db.BlogPosts.FindAll(db.BlogPosts.User.Email == email).WithUser().ToList<BlogPost>();
    }

    public BlogPost LoadById(int blogId)
    {
      var db = GetDatabase();

      return db.BlogPosts.FindAll(db.BlogPosts.Id == blogId).WithUser().FirstOrDefault();
    }

    public BlogPost Load(int year, int month, int day, string slug)
    {
      var db = GetDatabase();

      var startDate = new DateTime(year, month, day, 0, 0, 0);
      var endDate = new DateTime(year, month, day, 23, 59, 59);

      return
        db.BlogPosts.FindAll(db.BlogPosts.PublishDate >= startDate && db.BlogPosts.PublishDate <= endDate &&
                             db.BlogPosts.Slug == slug).WithUser().FirstOrDefault();
    }

    public BlogPost Save(BlogPost blogpost)
    {
      var db = GetDatabase();

      if (blogpost.Id == 0)
      {
        blogpost = db.BlogPosts.Insert(
          new
            {
              blogpost.Title,
              blogpost.Slug,
              blogpost.PublishDate,
              blogpost.CreationDate,
              blogpost.Published,
              UserId = blogpost.User.Id,
              blogpost.Content
            });
      }
      else
      {
        db.BlogPosts.Update(blogpost);
      }

      if (blogpost.Published)
        blogSearchService.Index(blogpost);
      else 
        blogSearchService.Delete(blogpost);

      return blogpost;
    }

    public void ToggleApproval(int blogId)
    {
      var post = LoadById(blogId);

      post.Published = !post.Published;

      Save(post);
    }

    public int Count()
    {
      var db = GetDatabase();

      return db.BlogPosts.Query(db.BlogPosts.Published == true).Count();
    }
  }
}
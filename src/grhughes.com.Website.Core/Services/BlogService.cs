namespace grhughes.com.Website.Core.Services
{
  using System;
  using System.Collections.Generic;
  using Interfaces;
  using Model;

  public class BlogService : BaseService, IBlogService
  {
    public void Delete(int blogId)
    {
      dynamic db = GetDatabase();

      db.BlogPosts.Delete(db.BlogPosts.Id == blogId);
    }

    public IList<BlogPost> Load(int page, int limit, bool loadAll = false)
    {
      dynamic db = GetDatabase();

      if (loadAll)
        return db.BlogPosts.All().OrderByPublishDateDescending().Skip(limit * page).Take(limit).ToList<BlogPost>();

      return db.BlogPosts.FindAll(db.BlogPosts.Published == true).OrderByPublishDateDescending().Skip(limit * page).Take(limit).ToList<BlogPost>();
    }

    public IList<BlogPost> LoadForUser(string email)
    {
      dynamic db = GetDatabase();

      return db.BlogPosts.FindAll(db.BlogPosts.Author.Email == email).ToList<BlogPost>();
    }

    public BlogPost LoadById(int blogId)
    {
      dynamic db = GetDatabase();

      return db.BlogPosts.Find(db.BlogPosts.Id == blogId);
    }

    public BlogPost Load(int year, int month, int day, string slug)
    {
      dynamic db = GetDatabase();

      var startDate = new DateTime(year, month, day, 0, 0, 0);
      var endDate = new DateTime(year, month, day, 23, 59, 59);

      return
        db.BlogPosts.Find(db.BlogPosts.PublishDate >= startDate && db.BlogPosts.PublishDate <= endDate &&
                          db.BlogPosts.Slug == slug);
    }

    public BlogPost Save(BlogPost blogpost)
    {
      dynamic db = GetDatabase();

      if (blogpost.Id == 0)
      {
        var post = db.BlogPosts.Insert(
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

        return LoadById(post.Id);
      }

      db.BlogPosts.Update(blogpost);
      return blogpost;
    }

    public void ToggleApproval(int blogId)
    {
      var post = LoadById(blogId);

      post.Published = !post.Published;

      Save(post);
    }
  }
}
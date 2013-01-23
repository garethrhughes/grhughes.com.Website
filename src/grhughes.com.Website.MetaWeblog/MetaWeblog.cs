namespace grhughes.com.Website.MetaWeblog
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using CookComputing.XmlRpc;
  using Core.Extensions;
  using Core.Model;
  using Core.Services;
  using Core.Services.Interfaces;

  public class MetaWeblog : XmlRpcService, IMetaWeblog
  {
    private readonly IBlogService blogService;
    private readonly IAuthenticationService authenticationService;

    public MetaWeblog()
    {
      blogService = new BlogService();
      authenticationService = new AuthenticationService();
    }

    #region IMetaWeblog Members

    string IMetaWeblog.AddPost(string blogid, string username, string password,
                               Post post, bool publish)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");

      var year = post.dateCreated.Year;

      var newPost = new BlogPost
                      {
                        Title = post.title,
                        Content = post.description,
                        PublishDate = year == 1 ? DateTime.Now : post.dateCreated.ToUniversalTime(),
                        CreationDate = DateTime.Now,
                        User = authenticationService.Load(username),
                        Published = false
                      };

      newPost.Slug = newPost.Title.Slug();

      return blogService.Save(newPost).Id.ToString();
    }

    bool IMetaWeblog.UpdatePost(string postid, string username, string password,
                                Post post, bool publish)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");

      var blog = blogService.LoadById(int.Parse(postid));

      if (blog.User.Email != username || blog.Published)
        throw new XmlRpcFaultException(0, "User does not have permission to edit this item.");

      blog.Title = post.title;
      blog.Slug = blog.Title.Slug();
      blog.Content = post.description;
      blog.PublishDate = post.dateCreated.ToUniversalTime();

      blogService.Save(blog);

      return true;
    }

    Post IMetaWeblog.GetPost(string postid, string username, string password)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");

      var blog = blogService.LoadById(int.Parse(postid));

      var post = new Post
                   {
                     postid = blog.Id,
                     title = blog.Title,
                     description = blog.Content,
                     dateCreated = blog.PublishDate.ToUniversalTime(),
                     userid = username,
                     wp_slug = blog.Slug
                   };

      return post;
    }

    CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
    {
      /*
      if (!authenticationService.Authenticate (username, password))
        throw new XmlRpcFaultException (0, "User is not valid!");
       */

      /*
       * TODO: Tagging
       */
      return new CategoryInfo[0];
    }

    Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
                                      int numberOfPosts)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");


      var blogs = blogService.LoadForUser(username).Take(10);

      return blogs.Select(blog => new Post
                                    {
                                      postid = blog.Id,
                                      title = blog.Title,
                                      description = blog.Content,
                                      dateCreated = blog.PublishDate,
                                      userid = blog.User.Email,
                                      wp_slug = blog.Slug,
                                    }).ToArray();
    }

    MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
                                               MediaObject mediaObject)
    {
      throw new NotImplementedException();
    }

    bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");

      var blog = blogService.LoadById(int.Parse(postid));

      if (!blog.Published && blog.User.Email == username)
        blogService.Delete(int.Parse(postid));
      else
        return false;

      return true;
    }

    BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");

      var infoList = new List<BlogInfo>
                       {
                         new BlogInfo
                           {
                             blogid = "1",
                             blogName = "grhughes.com",
                             url = "http://grhughes.com/"
                           }
                       };

      return infoList.ToArray();
    }

    UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
    {
      if (!authenticationService.Authenticate(username, password))
        throw new XmlRpcFaultException(0, "User is not valid!");

      var user = authenticationService.Load(username);
      var name = user.Name.Split(' ');

      return new UserInfo
               {
                 firstname = name.Length > 0 ? name[0] : string.Empty,
                 lastname = name.Length > 1 ? name[1] : string.Empty,
                 email = user.Email,
                 userid = user.Email
               };
    }

    #endregion
  }
}
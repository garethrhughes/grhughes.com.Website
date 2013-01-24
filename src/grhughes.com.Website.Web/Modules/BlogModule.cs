namespace grhughes.com.Website.Web.Modules
{
  using Core.Model;
  using Core.Services.Interfaces;
  using Nancy;
  using Helpers;
  using Nancy.Responses;

  public class BlogModule : BaseModule 
  {
    private readonly IBlogService blogService;

    public BlogModule(IBlogService blogService)
    {
      this.blogService = blogService;

      Get["/"] = _ =>
                   {
                     return View["Index", new BlogViewModel
                                           {
                                             Page = 0,
                                             Posts = this.blogService.Load(0, 5),
                                             TotalPosts = this.blogService.Count(),
                                             Limit = 5
                                           }];
                   };

      Get[@"/page/(?<page>[\d]+)"] = p =>  {
                     return View["Index", new BlogViewModel
                                           {
                                             Page = p.Page,
                                             Posts = this.blogService.Load((int)p.Page, 5),
                                             TotalPosts = this.blogService.Count(),
                                             Limit = 5
                                           }];
                   };

      Get["/rss"] = _ => View["RSS", this.blogService.Load(0)];
      Get["/rss.xml"] = _ => View["RSS", this.blogService.Load(0)];

      Get[@"/{year}/{month}/{day}/{slug}.html"] = p =>
                                                    {
                                                      var blog = this.blogService.Load((int) p.year, (int) p.month,
                                                                                       (int) p.day, (string) p.slug);

                                                      if (blog == null) return 404;

                                                      if (Context.CurrentUser == null && !blog.Published)
                                                        return 404;

                                                      return View["View", blog];
                                                    };

      Get[@"/{id}/{slug}"] = p =>
                               {
                                 var blog = this.blogService.LoadById((int) p.Id);
                                 if (blog == null || blog.Slug != p.slug.ToString()) return 404;

                                 return Response.AsRedirect(blog.GetUrl(), RedirectResponse.RedirectType.Permanent);
                               };

     
    }
  }
}
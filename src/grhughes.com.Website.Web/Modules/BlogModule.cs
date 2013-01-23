namespace grhughes.com.Website.Web.Modules
{
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

      Get["/"] = _ => View["Index", this.blogService.Load(0)];
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

      Get[@"/(?<page>[\d]+)"] = p => View["Index", this.blogService.Load(p.page)];
    }
  }
}
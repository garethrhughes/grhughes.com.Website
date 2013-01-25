namespace grhughes.com.Website.Web.Modules
{
  using Core.Model;
  using Core.Services.Interfaces;
  using Helpers;
  using Nancy;
  using Nancy.Responses;
  using Nancy.Responses.Negotiation;

  public class BlogModule : BaseModule
  {
    private readonly IBlogService blogService;

    public BlogModule(IBlogService blogService)
    {
      this.blogService = blogService;

      Get["/blog"] = _ => Response.AsRedirect("/", RedirectResponse.RedirectType.Permanent);
      Get["/blog/1"] = _ => Response.AsRedirect("/", RedirectResponse.RedirectType.Permanent);

      Get["/"] = _ => BlogIndex();
      Get[@"/page/(?<page>[\d]+)"] = p => BlogIndex((int) p.Page);

      Get["/rss"] = _ => BlogRSS();
      Get["/rss.xml"] = _ => BlogRSS();

      Get[@"/{year}/{month}/{day}/{slug}.html"] = p => BlogPage(p);
      Get[@"/{id}/{slug}"] = p => BlogPageRedirect(p);
    }

    private Negotiator BlogRSS()
    {
      return View["RSS", blogService.Load(0)].WithContentType("text/xml");
    }

    private dynamic BlogIndex(int page = 0)
    {
      return View["Index", new BlogViewModel
                             {
                               Page = page,
                               Posts = blogService.Load(page, 5),
                               TotalPosts = blogService.Count(),
                               Limit = 5
                             }];
    }

    private dynamic BlogPageRedirect(dynamic p)
    {
      var blog = blogService.LoadById((int) p.Id);
      if (blog == null || blog.Slug != p.slug.ToString()) return 404;

      return Response.AsRedirect(blog.GetUrl(), RedirectResponse.RedirectType.Permanent);
    }

    private dynamic BlogPage(dynamic p)
    {
      var blog = blogService.Load((int) p.year, (int) p.month,
                                  (int) p.day, (string) p.slug);

      if (blog == null) return 404;

      if (Context.CurrentUser == null && !blog.Published)
        return 404;

      return View["View", blog];
    }
  }
}
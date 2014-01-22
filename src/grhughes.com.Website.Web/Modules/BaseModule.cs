namespace grhughes.com.Website.Web.Modules
{
  using Core.Services.Interfaces;
  using Nancy;

  public class BaseModule : NancyModule
  {
    private readonly IBlogService _blogService;

    public BaseModule(IBlogService blogService)
    {
      _blogService = blogService;
      SetupHandlers();
    }

    public BaseModule(string modulePath, IBlogService blogService)
      : base(modulePath)
    {
      _blogService = blogService;
      SetupHandlers();
    }

    private void SetupHandlers()
    {
      After += ctx =>
      {
        ctx.ViewBag.HideCookieNotice = ctx.Request.Cookies.ContainsKey("CookiePolicyAccepted");
        ctx.ViewBag.LatestPosts = _blogService.Load(0, 15);
      };
    }
  }
}
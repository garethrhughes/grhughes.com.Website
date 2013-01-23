namespace grhughes.com.Website.Web.Modules
{
  using Nancy;

  public class BaseModule : NancyModule
  {
    public BaseModule()
    {
      SetupCookieHandler();
    }

    public BaseModule(string modulePath)
      : base(modulePath)
    {
      SetupCookieHandler();
    }

    private void SetupCookieHandler()
    {
      After += ctx =>
                 {
                   ctx.ViewBag.HideCookieNotice = ctx.Request.Cookies.ContainsKey("CookiePolicyAccepted");
                 };
    }
  }
}
namespace grhughes.com.Website.Web.Modules
{
  using Core.Services.Interfaces;

  public class ProfileModule : BaseModule
  {
    public ProfileModule(IBlogService blogService) :base(blogService)
    {
      Get["/profile"] = _ => View["Index"];

      After += ctx =>
      {
        ctx.ViewBag.Active = "Profile";
      };
    }
  }
}
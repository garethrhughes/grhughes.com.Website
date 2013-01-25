namespace grhughes.com.Website.Web.Modules
{
  public class ProfileModule : BaseModule
  {
    public ProfileModule ()
    {
      Get["/profile"] = _ => View["Index"];

      After += ctx =>
      {
        ctx.ViewBag.Active = "Profile";
      };
    }
  }
}
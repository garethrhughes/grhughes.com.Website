namespace grhughes.com.Website.Web.Modules
{
  public class PortfolioModule : BaseModule
  {
    public PortfolioModule()
    {
      Get["/portfolio"] = _ => View["Index"];

      After += ctx =>
      {
        ctx.ViewBag.Active = "Portfolio";
      };
    }
  }
}
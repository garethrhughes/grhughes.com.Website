namespace grhughes.com.Website.Web.Modules
{
  using Core.Services.Interfaces;

  public class PortfolioModule : BaseModule
  {
    public PortfolioModule(IBlogService blogService) : base(blogService)
    {
      Get["/portfolio"] = _ => View["Index"];

      After += ctx =>
      {
        ctx.ViewBag.Active = "Portfolio";
      };
    }
  }
}
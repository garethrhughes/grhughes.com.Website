namespace grhughes.com.Website.Web.Modules
{
  using Core.Services.Interfaces;
  using Nancy;
  using Nancy.Security;

  public class ManagementModule : BaseModule
  {
    public ManagementModule(IBlogService blogService) : base("/manage")
    {
      this.RequiresAuthentication();

      Get["/"] = _ => View["Index.spark", blogService.LoadAll()];
      Get["/approve/{id}"] = p =>
                               {
                                 blogService.ToggleApproval((int) p.id);

                                 return Response.AsRedirect("/manage");
                               };
    }
  } 
}
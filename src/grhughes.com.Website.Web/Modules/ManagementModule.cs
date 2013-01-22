namespace grhughes.com.Website.Web.Modules
{
  using Core.Services.Interfaces;
  using Nancy;
  using Nancy.Security;

  public class ManagementModule : NancyModule
  {
    private readonly IBlogService blogService;

    public ManagementModule(IBlogService blogService) : base("/manage")
    {
      this.blogService = blogService;
      this.RequiresAuthentication();

      Get["/articles"] = _ => View["Index.spark", this.blogService.Load(0, 100, true)];
      Get["/users"] = _ => "Boom 2";
      Get["/approve/{id}"] = p =>
                               {
                                 blogService.ToggleApproval((int) p.id);

                                 return Response.AsRedirect("/manage/articles");
                               };
    }
  }
}
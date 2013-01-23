namespace grhughes.com.Website.Web.Modules
{
  using Core.Services.Interfaces;
  using Nancy;
  using Nancy.Security;

  public class ManagementModule : BaseModule
  {
    private readonly IBlogService blogService;

    public ManagementModule(IBlogService blogService) : base("/manage")
    {
      this.blogService = blogService;
      this.RequiresAuthentication();

      Get["/articles"] = _ => View["Index.spark", this.blogService.LoadAll()];
      Get["/users"] = _ => "Boom 2";
      Get["/approve/{id}"] = p =>
                               {
                                 blogService.ToggleApproval((int) p.id);

                                 return Response.AsRedirect("/manage/articles");
                               };
    }
  }
}
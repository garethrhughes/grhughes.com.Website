namespace grhughes.com.Website.Web
{
  using System.Configuration;
  using System.IO;
  using System.Threading.Tasks;
  using System.Web.Hosting;
  using Core.Model;
  using Core.Services;
  using Core.Services.Interfaces;
  using Nancy;
  using Nancy.Authentication.Forms;
  using Nancy.Bootstrapper;
  using Nancy.Conventions;
  using Nancy.Diagnostics;
  using Nancy.Session;
  using Nancy.TinyIoc;

  public class Bootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      container.Register<ISearchService<BlogPost>, SearchService>();

      if (!Directory.Exists(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["IndexPath"])))
      {
        var searchService = container.Resolve<ISearchService<BlogPost>>();
        var blogService = container.Resolve<IBlogService>();
        new Task(() => searchService.Index(blogService.LoadAll())).Start();
      }

      DiagnosticsHook.Disable(pipelines);
    }

    protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
    {
      CookieBasedSessions.Enable(pipelines);

      var formsAuthConfiguration = new FormsAuthenticationConfiguration
                                     {
                                       RedirectUrl = "~/login",
                                       UserMapper = container.Resolve<IUserMapper>(),
                                     };

      FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
    }

    protected override void ConfigureConventions(NancyConventions nancyConventions)
    {
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("css", @"css"));
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("js", @"js"));
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("img", @"img"));
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddFile("/robots.txt", "/robots.txt"));

      StaticConfiguration.CaseSensitive = false;
      StaticConfiguration.DisableErrorTraces = true;
 

      base.ConfigureConventions(nancyConventions);
    }
  }
}
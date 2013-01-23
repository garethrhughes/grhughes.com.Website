namespace grhughes.com.Website.Web
{
  using System.Threading.Tasks;
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
      var searchService = container.Resolve<ISearchService>();

      new Task(searchService.ReIndex).Start();
    }

    protected override DiagnosticsConfiguration DiagnosticsConfiguration
    {
      get { return new DiagnosticsConfiguration {Password = @"password"}; }
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
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("font", @"font"));
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("ico", @"ico"));
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("js", @"js"));
      nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("img", @"img"));

      StaticConfiguration.CaseSensitive = false;
      StaticConfiguration.DisableErrorTraces = false;

      base.ConfigureConventions(nancyConventions);
    }
  }
}
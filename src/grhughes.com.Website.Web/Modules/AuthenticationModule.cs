namespace grhughes.com.Website.Web.Modules
{
  using System;
  using System.Text.RegularExpressions;
  using Core.Model;
  using Core.Services.Interfaces;
  using Nancy;
  using Nancy.Authentication.Forms;
  using Nancy.ModelBinding;

  public class AuthenticationModule : BaseModule
  {
    private readonly IAuthenticationService authenticationService;

    public AuthenticationModule(IAuthenticationService authenticationService)
    {
      this.authenticationService = authenticationService;

      Get["/register"] = _ => View["Register"];
      Get["/login"] = _ => View["Login"];
      Get["/logout"] = _ => this.LogoutAndRedirect("~/");

      Get["/oauth-callback"] = _ => OAuthCallback();
      Post["/login"] = _ => Login();
    }

    private dynamic Login()
    {
      var login = this.Bind<LoginDetails>();

      if (authenticationService.Authenticate(login.Username, login.Password))
      {
        var user = authenticationService.Load(login.Username);

        return this.LoginAndRedirect(user.UserId, DateTime.Now.AddDays(7));
      }

      return View["LoginError"];
    }

    private dynamic OAuthCallback()
    {
      {
        var code = this.Bind<ResponseCode>();
        var token = authenticationService.GetAccessToken(code.Code);
        var userData = authenticationService.GetUserDetails(token);

        var regex = new Regex("([^@])+@grhughes.com");
        if (!regex.IsMatch(userData.email))
          return 500;

        var blogAccessCode = authenticationService.AddUser(userData, code.Code);

        return View["Registered", new
                                    {
                                      data = userData,
                                      code = blogAccessCode
                                    }];
      }
    }
  }
}
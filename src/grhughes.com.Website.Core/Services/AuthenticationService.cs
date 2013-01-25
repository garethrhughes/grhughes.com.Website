namespace grhughes.com.Website.Core.Services
{
  using System;
  using System.Configuration;
  using System.IO;
  using System.Net;
  using DevOne.Security.Cryptography.BCrypt;
  using Interfaces;
  using Model;
  using Nancy;
  using Nancy.Authentication.Forms;
  using Nancy.Security;
  using Newtonsoft.Json;
  using Security;

  public class AuthenticationService : BaseService, IAuthenticationService, IUserMapper
  {
    public User Load(string username)
    {
      dynamic db = GetDatabase();

      return db.Users.FindByEmail(username);
    }

    public User Load(Guid id)
    {
      dynamic db = GetDatabase();

      return db.Users.FindByUserId(id);
    }

    public bool Authenticate(string username, string password)
    {
      dynamic db = GetDatabase();

      var user = db.Users.FindByEmail(username);

      return BCryptHelper.CheckPassword(password, user.Password);
    }

    public string AddUser(OAuthUserDetails userDetails, string code)
    {
      dynamic db = GetDatabase();

      var salt = BCryptHelper.GenerateSalt();
      var password = PasswordUtil.GeneratePassword();
      var hashedPassword = PasswordUtil.HashPassword(password, salt);

      var user = db.Users.FindByEmail(userDetails.email);

      if (user == null)
      {
        db.Users.Insert(new User
                          {
                            Email = userDetails.email,
                            Name = userDetails.name,
                            AppsCode = code,
                            Salt = salt,
                            Password = hashedPassword
                          });
      }
      else
      {
        user.Salt = salt;
        user.Password = hashedPassword;

        db.Users.Update(user);
      }

      return password;
    }

    public OAuthAccessRequest GetAccessToken(string code)
    {
      var request = WebRequest.Create("https://accounts.google.com/o/oauth2/token");
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      var writer = new StreamWriter(request.GetRequestStream());
      writer.Write(
        string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                      code,
                      ConfigurationManager.AppSettings["OAuthClientId"],
                      ConfigurationManager.AppSettings["OAuthSecret"],
                      ConfigurationManager.AppSettings["OAuthRedirect"]));

      writer.Close();


      var response = request.GetResponse();

      var reader = new StreamReader(response.GetResponseStream());

      return JsonConvert.DeserializeObject<OAuthAccessRequest>(reader.ReadToEnd());
    }

    public OAuthUserDetails GetUserDetails(OAuthAccessRequest requestToken)
    {
      var request =
        WebRequest.Create(string.Format("https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}",
                                        requestToken.access_token));
      request.Method = "GET";

      var response = request.GetResponse();
      var reader = new StreamReader(response.GetResponseStream());

      return JsonConvert.DeserializeObject<OAuthUserDetails>(reader.ReadToEnd());
    }

    public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
    {
      var user = Load(identifier);

      return new UserIdentity {UserName = user.Email};
    }
  }
}
namespace grhughes.com.Website.Core.Services.Interfaces
{
  using Model;

  public interface IAuthenticationService
  {
    OAuthAccessRequest GetAccessToken(string code);

    OAuthUserDetails GetUserDetails(OAuthAccessRequest requestToken);

    bool Authenticate(string username, string password);

    string AddUser(OAuthUserDetails userDetails, string code);

    User Load(string username);
  }
}
namespace grhughes.com.Website.Core.Model
{
  public class OAuthAccessRequest
  {
    public string access_token { get; set; }

    public int expires_in { get; set; }

    public string token_type { get; set; }
  }
}
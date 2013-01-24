namespace grhughes.com.Website.Web.Helpers
{
  using System.Configuration;
  using System.Web;
  using Core.Model;

  public static class UrlHelper
  {
    public static string GoogleAccessUrl
    {
      get
      {
        return
          string.Format(
            "https://accounts.google.com/o/oauth2/auth?response_type=code&client_id={0}&redirect_uri={1}&scope={2}",
            ConfigurationManager.AppSettings["OAuthClientId"], ConfigurationManager.AppSettings["OAuthRedirect"],
            HttpUtility.UrlEncode(
              "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email"));
      }
    }

    public static string GetUrl(this BlogPost post)
    {
      return string.Format("/{0}/{1}.html", post.PublishDate.ToString("yyyy/MM/dd"), post.Slug);
    }

    public static string ForSearch(string query, int page = 0)
    {
      if (page == 0)
        return string.Format("/search?query={0}", query);

      return string.Format("/search?query={0}&page={1}", query, page);
    }

    public static string ForPage(int page = 0)
    {
      if (page == 0)
        return string.Format("/");

      return string.Format("/page/{0}", page);
    }

    public static string GetApprovalUrl(this BlogPost post)
    {
      return string.Format("/manage/approve/{0}", post.Id);
    }
  }
}
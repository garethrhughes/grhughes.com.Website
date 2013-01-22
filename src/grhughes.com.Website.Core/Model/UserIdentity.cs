namespace grhughes.com.Website.Core.Model
{
  using System.Collections.Generic;
  using Nancy.Security;

  public class UserIdentity : IUserIdentity
  {
    public string UserName { get; set; }

    public IEnumerable<string> Claims { get; set; }
  }
}
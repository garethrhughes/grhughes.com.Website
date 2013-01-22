namespace grhughes.com.Website.Core.Model
{
  using System;

  public class User
  {
    public User()
    {
      UserId = Guid.NewGuid();
    }

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public string AppsCode { get; set; }
  }
}
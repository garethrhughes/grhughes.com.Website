namespace grhughes.com.Website.Core.Security
{
  using System;
  using System.Linq;
  using DevOne.Security.Cryptography.BCrypt;

  public static class PasswordUtil
  {
    public static string HashPassword(string password, string salt)
    {
      return BCryptHelper.HashPassword(password, salt);
    }

    public static string GeneratePassword()
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      var random = new Random();
      var result = new string(
        Enumerable.Repeat(chars, 8)
          .Select(s => s[random.Next(s.Length)])
          .ToArray());

      return result;
    }
  }
}
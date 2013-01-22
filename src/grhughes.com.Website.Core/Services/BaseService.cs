namespace grhughes.com.Website.Core.Services
{
  using Simple.Data;

  public abstract class BaseService
  {
    protected dynamic GetDatabase()
    {
      return Database.Open();
    }
  }
}
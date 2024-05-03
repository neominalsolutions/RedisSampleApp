using RedisSampleApp.Models;

namespace RedisSampleApp.Redis
{
  public class UserSessionService
  {
    private readonly ICache _cache;
    private readonly string CacheKeyPrefix = "User";

    public UserSessionService(ICache cache)
    {
      _cache = cache;
    }

    public void SetSession(UserSession session, int expireAt = 30)
    {
      _cache.SetObject($"{CacheKeyPrefix}-{session.Id}", session, expireAt);
    }

    public UserSession? GetSession(string userId)
    {
      var value = _cache.GetObject<UserSession>($"{CacheKeyPrefix}-{userId}");

      if(value is null)
      {
        // find userId from Db
        // setCache
        // return userSession
      }

      return value;
    }
  }
}

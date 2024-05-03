using StackExchange.Redis;

namespace RedisSampleApp.Redis
{
  public class RedisCacheRepo : ICache
  {
    // Redis bağlantı nesnesi
    private readonly IConnectionMultiplexer _connection;
    private readonly IDatabase _database;


    public RedisCacheRepo(IConnectionMultiplexer connection)
    {
      _connection = connection;
      _database = _connection.GetDatabase();
    }

    // Tüm Cache temizler
    public void Clear()
    {
      _database.Execute("FLUSHDB");
    }

    public void Delete(string key)
    {
      // ilgili cache key silinir
      _database.KeyDelete(key);
    }

    public TObject GetObject<TObject>(string key) where TObject : class
    {
      // veriler string tipte saklandığı için Json ile çalışıp objkect formatına dönüştürelim.
      var cacheValue = _database.StringGet(key);

      if(cacheValue.HasValue)
      {
        return System.Text.Json.JsonSerializer.Deserialize<TObject>(cacheValue);
      }

      return null;
    }

    public void SetObject<TObject>(string key, TObject @object,  int expireAt = 30) where TObject : class
    {
      var cacheValue = System.Text.Json.JsonSerializer.Serialize(@object);
      _database.StringSet(key, cacheValue,TimeSpan.FromMinutes(expireAt));
    }
  }
}

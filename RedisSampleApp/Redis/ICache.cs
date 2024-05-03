namespace RedisSampleApp.Redis
{
  public interface ICache
  {

    // Cache Expire time set ederken belirtilir.
    public void SetObject<TObject>(string key,TObject @object, int expireAt=30) where TObject:class;
    public TObject GetObject<TObject>(string key) where TObject : class;

    public void Delete(string key);

    // Tüm Cache temizlemek için
    public void Clear(); 

  }
}

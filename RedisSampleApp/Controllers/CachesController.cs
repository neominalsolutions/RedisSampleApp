using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisSampleApp.Models;
using RedisSampleApp.Redis;
using StackExchange.Redis;

namespace RedisSampleApp.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CachesController : ControllerBase
  {
    private readonly ICache _cache;
    private readonly UserSessionService userSessionService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    

    public CachesController(ICache cache, UserSessionService userSessionService, IConnectionMultiplexer connectionMultiplexer)
    {
      _cache = cache;
      this.userSessionService = userSessionService;
      _connectionMultiplexer = connectionMultiplexer;
    }

    [HttpPost]
    public async Task<IActionResult> SetCache()
    {
      var random = new Random();
      var index = random.Next(0, 100);

      var user = new UserSession
      {
        Id = Guid.NewGuid(),
        UserName = $"test-{index}",
        Email = $"test-{index}@test.com",
        PhoneNumber = "3225324324"
      };

      //_cache.SetObject($"user-{user.Id}", user,60);
      userSessionService.SetSession(user);
      return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetCache(string userId)
    {
      // var session = _cache.GetObject<UserSession>($"user-{userId}");
      var session = userSessionService.GetSession(userId);
      return Ok(session);
    }


    [HttpPost("stockChange")]
    public async Task<IActionResult> PuslishEvent()
    {

      var random = new Random();
      int old = random.Next(1, 100);
      int @new = random.Next(1, 100);

      var jsonData = System.Text.Json.JsonSerializer.Serialize(new StockEvent { ProductId = "1", OldStock = old, NewStock = @new });


      await _connectionMultiplexer.GetDatabase().PublishAsync("stock-changed", jsonData);



      return Ok();
    }

  }
}

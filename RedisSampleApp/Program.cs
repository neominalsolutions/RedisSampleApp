using RedisSampleApp.Models;
using RedisSampleApp.Redis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var redisConnection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisLocal"));
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);
builder.Services.AddSingleton<ICache, RedisCacheRepo>();

// Not: Sesison Bazlý veri servisi olmasý sebebi ile UserSessionService Signleton olarak tutmayalým.
builder.Services.AddTransient<UserSessionService>();


var subs = redisConnection.GetSubscriber();

subs.Subscribe("stock-changed", (channel, message) =>
{
  var data = System.Text.Json.JsonSerializer.Deserialize<StockEvent>(message); 
  Console.WriteLine($"Stock Deðiþti Yeni Deðer {data.NewStock} Eski Deðer {data.OldStock} ");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

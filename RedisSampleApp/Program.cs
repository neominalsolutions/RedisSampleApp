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

// Not: Sesison Bazl� veri servisi olmas� sebebi ile UserSessionService Signleton olarak tutmayal�m.
builder.Services.AddTransient<UserSessionService>();


var subs = redisConnection.GetSubscriber();

subs.Subscribe("stock-changed", (channel, message) =>
{
  var data = System.Text.Json.JsonSerializer.Deserialize<StockEvent>(message); 
  Console.WriteLine($"Stock De�i�ti Yeni De�er {data.NewStock} Eski De�er {data.OldStock} ");
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

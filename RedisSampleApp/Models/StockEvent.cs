namespace RedisSampleApp.Models
{
  public class StockEvent
  {
    public string ProductId { get; set; }
    public decimal OldStock { get; set; }
    public decimal NewStock { get; set; }

  }
}

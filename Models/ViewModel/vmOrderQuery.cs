public class vmOrderQuery
{
  public Orders MasterData { get; set; } = new Orders();
  public List<OrderDetails> DetailData { get; set; } = new List<OrderDetails>();
}

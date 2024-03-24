namespace WebApi.Entities;

public class Stock
{
    public string Symbol { get; set; }
    public int Quantity { get; set; }
    public decimal AveragePrice { get; set; }

    public Stock(string symbol, int quantity, decimal averagePrice)
    {
        Symbol = symbol;
        Quantity = quantity;
        AveragePrice = averagePrice;
    }
}

namespace Entities;

public class Wallet
{
    public decimal Balance { get; set; } = 0;
    public IList<Stock> Stocks { get; set; } = [];

    public void AddFunds(decimal amountt)
    {
        Balance += amountt;
    }

    public void AddStock(string symbol, int quantity, decimal price)
    {
        //var totalCost = quantity * price;
        //if (totalCost > Balance)
        //{
        //    throw new Exception("Insufficient funds in your wallet");
        //}

        //Stocks.Add(new Stock2(symbol, quantity, price));
        //Balance -= totalCost;

        decimal currentTotalCost = quantity * price;
        if (currentTotalCost > Balance)
        {
            throw new Exception("Insufficient funds in your wallet");
        }

        var stockAlreadyBought = Stocks.FirstOrDefault(x => x.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase));


        if (stockAlreadyBought is null)
        {
            Stocks.Add(new Stock(symbol, quantity, price));
        }
        else
        {
            var previousTotalCost = stockAlreadyBought.Quantity * stockAlreadyBought.AveragePrice;

            var newQuantity = stockAlreadyBought.Quantity + quantity;
            var newAveragePrice = (previousTotalCost + currentTotalCost) / newQuantity;

            stockAlreadyBought.Quantity = newQuantity;
            stockAlreadyBought.AveragePrice = newAveragePrice;
        }

        RemoveFunds(currentTotalCost);
    }

    public void RemoveFunds(decimal amount)
    {
        if (amount > Balance)
        {
            Balance = 0;
        }
        else
        {
            Balance -= amount;
        }
    }

    public void RemoveStock(string symbol, int quantity, decimal price)
    {
        var stock = Stocks.FirstOrDefault(x => x.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase)) ?? throw new Exception("Stock not found in your wallet");

        var sellValue = quantity * price;

        AddFunds(sellValue);

        if (quantity == stock.Quantity)
        {
            // All stocks were sold
            Stocks.Remove(stock);
        }
        else
        {
            // Part of stocks were sold
            stock.Quantity -= quantity;
        }
    }
}

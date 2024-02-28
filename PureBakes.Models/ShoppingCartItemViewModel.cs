namespace PureBakes.Models;

public class ShoppingCartItemViewModel
{
    public Product Product;
    public int Count { get; set; }
    double TotalPrice { get; set; }
}
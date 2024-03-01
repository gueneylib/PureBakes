namespace PureBakes.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    [ValidateNever]
    public Product? Product { get; set; }

    public int ShoppingCartId { get; set; }

    [ForeignKey(nameof(ShoppingCartId))]
    [ValidateNever]
    public ShoppingCart? ShoppingCart { get; set; }

    public int Quantity { get; set; } = 0;
}
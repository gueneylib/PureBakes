namespace PureBakes.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class ShoppingCart
{
    public int Id { get; set; }

    [ValidateNever]
    public ICollection<ShoppingCartItem> ShoppingCartItem { get; set; } = new List<ShoppingCartItem>();

    public string PureBakesUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(PureBakesUserId))]
    [ValidateNever]
    public PureBakesUser? PureBakesUser { get; set; }

    [NotMapped]
    public double TotalPrice { get; set; }
}
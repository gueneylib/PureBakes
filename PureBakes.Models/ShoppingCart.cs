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

// How can i solve this?
// Create a list of products here for mapping
// create a factory to create itemviewmodel
// dont reference itemvm here because it has to be mapped.

// do it like always: get the cart and the products related to the cart id. then call the factory to create the itemvm with count etc.
// then use this for the view.


// three tables:
// Shoppingcart with only one user id, list of ShoppingCartItem (foreign key id of the ShoppingCartItem table)
// ShoppingCartItem with one Product (foreign key of Product id) and one quantity of the product
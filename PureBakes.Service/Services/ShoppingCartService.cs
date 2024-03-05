namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Service.Services.Interface;
using PureBakes.Models;

public class ShoppingCartService(
    IShoppingCartRepository shoppingCartRepository,
    IShoppingCartItemRepository shoppingCartItemRepository,
    IIdentityService identityService) : IShoppingCartService
{
    public ShoppingCart GetShoppingCartByUserId(string userId)
    {
        var shoppingCartOfUser = shoppingCartRepository
            .Get(x => x.PureBakesUserId == userId,
                nameof(ShoppingCart.ShoppingCartItem));

        if (shoppingCartOfUser is null)
        {
            return CreateShoppingCartForUser(userId);
        }

        return shoppingCartOfUser;
    }

    public int GetShoppingCartProductsQuantity()
    {
        var currentCartProductsCount = GetAllProductsInCart().Sum(product => product.Quantity);

        return currentCartProductsCount;
    }

    public IEnumerable<ShoppingCartItem> GetAllProductsInCart()
    {
        var userId = identityService.GetUserId();
        var shoppingCartOfUser = GetShoppingCartByUserId(userId);
        return shoppingCartItemRepository
            .GetAll(u => u.ShoppingCartId == shoppingCartOfUser.Id,
                includeProperties: nameof(Product));
    }

    public void UpdateCartItem(ShoppingCartItem match)
    {
        shoppingCartItemRepository.Update(match);
        shoppingCartItemRepository.Save();
    }

    private ShoppingCart CreateShoppingCartForUser(string userId)
    {
        var newCart = new ShoppingCart
        {
            PureBakesUserId = userId
        };
        shoppingCartRepository.Add(newCart);
        shoppingCartRepository.Save();

        return newCart;
    }
}
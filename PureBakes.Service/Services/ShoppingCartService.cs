namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Service.Services.Interface;
using PureBakes.Models;

public class ShoppingCartService(
    IShoppingCartRepository shoppingCartRepository,
    IShoppingCartItemRepository shoppingCartItemRepository,
    IIdentityService identityService) : IShoppingCartService
{
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

    public void UpdateCartItem(ShoppingCartItem shoppingCartItem)
    {
        var userId = identityService.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var shoppingCartOfUser = GetShoppingCartByUserId(userId);

        var itemInCart = shoppingCartOfUser.ShoppingCartItem.FirstOrDefault(x =>
            x.ProductId == shoppingCartItem.ProductId);
        if (itemInCart is not null)
        {
            itemInCart.Quantity += shoppingCartItem.Quantity;
            shoppingCartItemRepository.Update(itemInCart);
        }
        else
        {
            shoppingCartItem.ShoppingCartId = shoppingCartOfUser.Id;
            shoppingCartItemRepository.Add(shoppingCartItem);
        }

        shoppingCartItemRepository.Save();
    }

    public bool IncrementProductQuantity(int cartItemId)
    {
        return IncrementOrDecrementProductQuantity(cartItemId, 1);
    }

    public bool DecrementProductQuantity(int cartItemId)
    {
        return IncrementOrDecrementProductQuantity(cartItemId, -1);
    }

    public bool RemoveProductFromCart(int cartItemId)
    {
        var cartItem = shoppingCartItemRepository.Get(cartItemId);
        if (cartItem is null)
        {
            return false;
        }

        shoppingCartItemRepository.Remove(cartItem);
        shoppingCartItemRepository.Save();
        return true;
    }

    private bool IncrementOrDecrementProductQuantity(int cartItemId, int quantity)
    {
        var cartItem = shoppingCartItemRepository.Get(cartItemId);
        if (cartItem is null)
        {
            return false;
        }

        cartItem.Quantity += quantity;
        shoppingCartItemRepository.Update(cartItem);
        shoppingCartItemRepository.Save();
        return true;
    }

    private ShoppingCart GetShoppingCartByUserId(string userId)
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
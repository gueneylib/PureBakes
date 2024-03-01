namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Service.Services.Interface;
using PureBakes.Models;

public class ShoppingCartService(IUnitOfWork unitOfWork) : IShoppingCartService
{
    public ShoppingCart GetShoppingCartByUserId(string userId)
    {
        var shoppingCartOfUser = unitOfWork
            .ShoppingCart
            .Get(x => x.PureBakesUserId == userId,
                nameof(ShoppingCart.ShoppingCartItem));

        if (shoppingCartOfUser is null)
        {
            return CreateShoppingCartForUser(userId);
        }

        return shoppingCartOfUser;
    }

    public void CreateShoppingCartForUserIfNecessary(string userId)
    {
        var userCart = unitOfWork.ShoppingCart.GetAll().FirstOrDefault(x => x.PureBakesUserId == userId);
        if (userCart is null)
        {
            _ = CreateShoppingCartForUser(userId);
        }
    }

    private ShoppingCart CreateShoppingCartForUser(string userId)
    {
        var newCart = new ShoppingCart
        {
            PureBakesUserId = userId
        };
        unitOfWork.ShoppingCart.Add(newCart);
        unitOfWork.Save();

        return newCart;
    }
}
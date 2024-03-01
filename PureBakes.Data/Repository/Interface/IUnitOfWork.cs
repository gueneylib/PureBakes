namespace PureBakes.Data.Repository.Interface;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    IShoppingCartRepository ShoppingCart { get; }
    IShoppingCartItemRepository ShoppingCartItem { get; set; }

    void Save();
}
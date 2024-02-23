namespace PureBakes.Data.Repository.Interface;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }

    void Save();
}
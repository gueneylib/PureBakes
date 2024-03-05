namespace PureBakes.Data;

using PureBakes.Models;

public class DbInitializer : IDbInitializer
{
    private readonly PureBakesDbContext dbContext;

    public DbInitializer(PureBakesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Initialize()
    {
        if (!dbContext.Categories.Any())
        {
            dbContext.Categories.AddRange(Categories.Select(c => c.Value));
        }

        if (!dbContext.Products.Any())
        {
            dbContext.AddRange(
                new Product{ Title = "Rye Sourdough Bread", Category = Categories["Bread"], Price = 5.99 },
                new Product{ Title = "Neapolitan Pizza", Category = Categories["Pizza"], Price = 8.99 }
            );
        }

        dbContext.SaveChanges();
    }

    private static Dictionary<string, Category>? categories;

    public static Dictionary<string, Category> Categories
    {
        get
        {
            if (categories == null)
            {
                var genresList = new Category[]
                {
                    new Category { Name = "Bread" },
                    new Category { Name = "Pizza" }
                };

                categories = new Dictionary<string, Category>();

                foreach (Category genre in genresList)
                {
                    if (genre.Name != null)
                    {
                        categories.Add(genre.Name, genre);
                    }
                }
            }

            return categories;
        }
    }
}
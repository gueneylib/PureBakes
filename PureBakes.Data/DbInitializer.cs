namespace PureBakes.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureBakes.Models;

public class DbInitializer(
    PureBakesDbContext dbContext,
    RoleManager<IdentityRole> roleManager) : IDbInitializer
{
    public void Initialize()
    {
        // migrations if they are not applied
        try
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
        catch (Exception)
        {
            // ignored
        }

        if (!dbContext.Categories.Any())
        {
            dbContext.Categories.AddRange(Categories.Select(c => c.Value));
        }

        if (!dbContext.Products.Any())
        {
            dbContext.AddRange(
                new Product{ Title = "Rye Sourdough Bread", Description = "Rye is very rich in flavour and is very healthy", Category = Categories["Bread"], Price = 5.99, ImageUrl = "/images/product/ryeSourdough.jpg" },
                new Product{ Title = "Ciabatta", Description = "Italian Bread with high hydration" , Category = Categories["Bread"], Price = 2.99, ImageUrl = "/images/product/ciabatta.jpg" },
                new Product{ Title = "Neapolitan Pizza", Description = "Pizza napoletana, the first pizza ever made!" , Category = Categories["Pizza"], Price = 8.99, ImageUrl = "/images/product/neapolitan.jpeg" },
                new Product{ Title = "Roman Pizza", Description = "Pizza a la roma" , Category = Categories["Pizza"], Price = 8.99, ImageUrl = "/images/product/romanPizza.jpg" },
                new Product{ Title = "Simit", Description = "turkish bagels with sesame." , Category = Categories["Bread"], Price = 0.99, ImageUrl = "/images/product/simit.jpg" },
                new Product{ Title = "Brezel", Description = "German knot-shaped pastry" , Category = Categories["Bread"], Price = 1.49, ImageUrl = "/images/product/brezel.jpg" }
            );
        }

        if (!roleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult()) {
            roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole("Employee")).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
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
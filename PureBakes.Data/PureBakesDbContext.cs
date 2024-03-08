using Microsoft.EntityFrameworkCore;
using PureBakes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PureBakes.Data;


public class PureBakesDbContext: IdentityDbContext
{
    public PureBakesDbContext(DbContextOptions<PureBakesDbContext> options) : base(options){}

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PureBakesUser> PureBakesUsers { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

}
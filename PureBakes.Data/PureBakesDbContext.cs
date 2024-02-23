using Microsoft.EntityFrameworkCore;
using PureBakes.Models;

namespace PureBakes.Data;
public class PureBakesDbContext: DbContext
{
    public PureBakesDbContext(DbContextOptions<PureBakesDbContext> options) : base(options){}

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

}
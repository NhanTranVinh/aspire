using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ProductDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
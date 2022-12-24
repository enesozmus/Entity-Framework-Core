using Microsoft.EntityFrameworkCore;

namespace EF_Core_Approaches;

class Program
{
  // static void Main(string[] args)
  static async Task Main(string[] args)
  {
    // ExampleDbContext context = new();
    // await context.Database.MigrateAsync();
  }

  // DbContext
  public class ExampleDbContext : DbContext
  {
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=TestDb; Integrated Security=True;");
    }
  }

  // Entity
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
  }

  // Entity
  public class Customer
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}
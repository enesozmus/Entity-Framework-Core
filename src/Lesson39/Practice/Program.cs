using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

#region Raw SQL Queries

#region FromSqlRaw()
AppDbContext _context = new();

List<Product>? products = await _context.Products
                                  .FromSqlRaw($"SELECT * FROM Products")
                                  .ToListAsync();

// var id = 4;
var id = new SqlParameter("@_Id", 4);
Product? products2 = await _context.Products
                              .FromSqlRaw($"SELECT * FROM Products WHERE Id = @_Id", id)
                              .FirstAsync();

// var price = 50;
var price = new SqlParameter("@_Price", 50);
List<Product>? products3 = await _context.Products
                                .FromSqlRaw($"SELECT * FROM Products WHERE Price > @_Price", price)
                                .ToListAsync();

#region SELECT COLUMNS

// Id,Name,Price
List<ProductEssential>? products4 = await _context.ProductEssentials
                                .FromSqlRaw($"SELECT Id,Name,Price FROM Products")
                                .ToListAsync();

// Name,Price
List<ProductEssential2>? products5 = await _context.ProductEssentials2
                                .FromSqlRaw($"SELECT Name,Price FROM Products")
                                .ToListAsync();

// with join
List<ProductWithFeature>? products6 = await _context.ProductWithFeatures
                              .FromSqlRaw($"SELECT product.Id, product.Name, product.Price, productFeature.Color, productFeature.Height FROM Products product INNER JOIN ProductFeatures productFeature ON product.Id = productFeature.Id")
                              .ToListAsync();

Console.WriteLine("...");
public class ProductEssential
{
  public int Id { get; set; }
  public string Name { get; set; }

  public decimal Price { get; set; }
}
public class ProductEssential2
{
  // modelBuilder.Entity < ProductEssential2>().HasNoKey();
  public string Name { get; set; }

  public decimal Price { get; set; }
}
public class ProductWithFeature
{
  public int Id { get; set; }
  public string Name { get; set; }
  public decimal Price { get; set; }
  public string Color { get; set; }
  public int Height { get; set; }
}
#endregion

#endregion

#endregion


#region Seed Datas
// AppDbContext _context = new();
// await _context.AddAsync<Category>(new() { Name = "A - Category" });
// await _context.AddAsync<Category>(new() { Name = "B - Category" });
// await _context.AddAsync<Category>(new() { Name = "C - Category" });
// await _context.AddAsync<Category>(new() { Name = "D - Category" });
// await _context.SaveChangesAsync();
// Console.WriteLine("over");


// AppDbContext _context = new();
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 1", Price = 100, Stock = 150, Barcode = 121, ProductFeature = new ProductFeature() { Color = "Red", Height = 200, Width = 100 } });
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 2", Price = 200, Stock = 250, Barcode = 122, ProductFeature = new ProductFeature() { Color = "Red", Height = 200, Width = 100 } });
// await _context.AddAsync<Product>(new() { CategoryId = 2, Name = "kalem 3", Price = 300, Stock = 350, Barcode = 123, ProductFeature = new ProductFeature() { Color = "Red", Height = 200, Width = 100 } });
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 4", Price = 400, Stock = 450, Barcode = 124 });
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 5", Price = 500, Stock = 550, Barcode = 125 });
// await _context.SaveChangesAsync();
// Console.WriteLine("over");

#endregion


#region Entites
public class Category
{
  public Category()
  {
    Products = new HashSet<Product>();
  }
  public int Id { get; set; }
  public string Name { get; set; }

  public ICollection<Product> Products { get; set; }
}
public class Product
{
  public int Id { get; set; }
  public string Name { get; set; }
  // #######.##
  [Precision(9, 2)]
  public decimal Price { get; set; }

  [Precision(9, 2)]
  public decimal DiscountPrice { get; set; }

  public int Stock { get; set; }

  public int Barcode { get; set; }
  public bool IsDeleted { get; set; }
  public int CategoryId { get; set; }
  public Category Category { get; set; }

  public ProductFeature ProductFeature { get; set; }
}
public class ProductFeature
{
  [Key, ForeignKey("Product")]
  public int Id { get; set; }

  public int Width { get; set; }
  public int Height { get; set; }
  public string Color { get; set; }

  public Product Product { get; set; }
}
#endregion


#region DbContext
public class AppDbContext : DbContext
{
  public AppDbContext() { }

  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<ProductFeature> ProductFeatures { get; set; }
  // need
  // bir veritabanı nesnesi değildir
  // ID'li
  public DbSet<ProductEssential> ProductEssentials { get; set; }
  // need
  // bir veritabanı nesnesi değildir
  // ID'siz
  // modelBuilder.Entity < ProductEssential2>().HasNoKey();
  public DbSet<ProductEssential2> ProductEssentials2 { get; set; }
  // need
  // bir veritabanı nesnesi değildir
  // ID'li
  public DbSet<ProductWithFeature> ProductWithFeatures { get; set; }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=RawSQLDb; Integrated Security=True;");
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<ProductEssential2>().HasNoKey();
    // modelBuilder.Entity<ProductEssential2>().HasNoKey().ToSqlQuery("SELECT NAME, PRICE FROM Products");
    // var products = _context.productEssentials.Where(x => x.Price > 200).ToList();
    // where cümlesini de sql'e basar
    modelBuilder.Entity<ProductWithFeature>().HasNoKey();
    modelBuilder.Entity<ProductWithFeature>().HasNoKey();

    base.OnModelCreating(modelBuilder);
  }
}
#endregion


#region packages and commands
// dotnet add package Microsoft.EntityFrameworkCore --version 6.0.12
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.12
// dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.12
// dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.12

// dotnet ef migrations add mig_1
// dotnet ef database update
#endregion
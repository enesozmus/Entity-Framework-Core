using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Join() - INNER JOIN
#region Method Syntax
AppDbContext _context = new();
var result = await _context.Categories
                            .Join
                            (
                              // kimi join yapmak istiyorsun
                              _context.Products,
                                // bağlantılar
                                category => category.Id,
                                product => product.CategoryId,
                              (category, product) => new
                              {
                                // oluşturmak istediğin yeni liste
                                CategoryName = category.Name,
                                ProductName = product.Name,
                                ProductPrice = product.Price,
                                KontrolSende = product.Barcode
                              }
                            ).ToListAsync();
// Triple
var result3 = await _context.Categories
                              .Join
                              (
                                // kimi join yapmak istiyorsun
                                _context.Products,
                                  category => category.Id,
                                  product => product.CategoryId,
                                (category, product) => new
                                // aşağidaki temsili up
                                { category, product }
                              )
                              .Join
                              (
                                // kimi join yapmak istiyorsun
                                _context.ProductFeatures,
                                  up => up.product.Id,
                                  productFeature => productFeature.Id,
                                (up, productFeature) => new
                                {
                                  CategoryName = up.category.Name,
                                  ProductName = up.product.Name,
                                  ProductPrice = up.product.Price,
                                  ProductColor = productFeature.Color
                                }
                              ).ToListAsync();
#endregion

#region Query Syntax
var result2 = from category in _context.Categories
              join product in _context.Products on category.Id equals product.CategoryId
              select new
              {
                CategoryName = category.Name,
                ProductName = product.Name,
                ProductPrice = product.Price,
                KontrolSende = product.Barcode
              };
// Triple
var result4 = await (from category in _context.Categories
                     join product in _context.Products on category.Id equals product.CategoryId
                     join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id
                     select new
                     {
                       CategoryName = category.Name,
                       ProductName = product.Name,
                       ProductPrice = product.Price,
                       ProductColor = productFeature.Color
                     }).ToListAsync();
#endregion
#endregion


#region from...join - LEFt JOIN & RIGHT JOIN
// özünde → Kendisine bir üründetay tablosu bağlanmamış ürünleri göreceğiz
// bir üründetay tablosu olanlar
var result5 = await (from product in _context.Products
                     join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id
                     select new { product }).ToListAsync();
// DefaultIfEmpty(): boşsa default'unu al
// özünde → Kendisine bir üründetay tablosu bağlanmış ve bağlanmamış ürünleri göreceğiz
var result6 = await (from product in _context.Products
                     join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id into productFeatureList
                     from productFeature in productFeatureList.DefaultIfEmpty()
                     select new { product }).ToListAsync();
// özünde → Kendisine bir üründetay tablosu bağlanmış ve bağlanmamış ürünleri + bağlı üründetay tablolarını + olmayanlar için null göreceğiz
var result7 = await (from product in _context.Products
                     join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id into productFeatureList
                     from productFeature in productFeatureList.DefaultIfEmpty()
                     select new { product, productFeature }).ToListAsync();

var leftJoinResult = await (from product in _context.Products
                            join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id
                            into productFeatureList
                            from productFeature in productFeatureList.DefaultIfEmpty()
                            select new
                            {
                              ProductName = product.Name,
                              ProductPrice = product.Price,
                              ProductColor = productFeature.Color,
                              ProductWidth = (int?)productFeature.Width,
                              ProductHeight = (int?)productFeature.Height == null ? 0 : productFeature.Height
                            }).ToListAsync();

var rightJoinResult = await (from productFeature in _context.ProductFeatures
                             join product in _context.Products on productFeature.Id equals product.Id
                             into productList
                             from product in productList.DefaultIfEmpty()
                             select new
                             {
                               ProductName = product.Name,
                               ProductPrice = (decimal?)product.Price,
                               ProductColor = productFeature.Color,
                               ProductWidth = productFeature.Width,
                               ProductHeight = productFeature.Height
                             }).ToListAsync();
#endregion


#region Full Outer Join
// cross + ürün + detay
var left = await (from product in _context.Products
                  join productFeature in _context.ProductFeatures on product.Id equals productFeature.Id
                  into productFeatureList
                  from productFeature in productFeatureList.DefaultIfEmpty()
                  select new
                  {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductColor = productFeature.Color,
                  }
                  ).ToListAsync();
var right = await (from productFeature in _context.ProductFeatures
                   join product in _context.Products on productFeature.Id equals product.Id
                   into productList
                   from product in productList.DefaultIfEmpty()
                   select new
                   {
                     ProductId = product.Id,
                     ProductName = product.Name,
                     ProductColor = productFeature.Color,
                   }
                  ).ToListAsync();

var full_join = left.Union(right);
Console.WriteLine("over");
#endregion


#region Seed Datas
// AppDbContext _context = new();
// await _context.AddAsync<Category>(new() { Name = "A - Category" });
// await _context.AddAsync<Category>(new() { Name = "B - Category" });
// await _context.AddAsync<Category>(new() { Name = "C - Category" });
// await _context.AddAsync<Category>(new() { Name = "D - Category" });
// await _context.SaveChangesAsync();
// Console.WriteLine("over");


// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 1", Price = 100, Stock = 200, Barcode = 123, ProductFeature = new ProductFeature() { Color = "Red", Height = 200, Width = 100 } });
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 2", Price = 100, Stock = 200, Barcode = 123, ProductFeature = new ProductFeature() { Color = "Red", Height = 200, Width = 100 } });
// await _context.AddAsync<Product>(new() { CategoryId = 2, Name = "kalem 3", Price = 100, Stock = 200, Barcode = 123, ProductFeature = new ProductFeature() { Color = "Red", Height = 200, Width = 100 } });
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 4", Price = 100, Stock = 200, Barcode = 123 });
// await _context.AddAsync<Product>(new() { CategoryId = 1, Name = "kalem 5", Price = 100, Stock = 200, Barcode = 123 });
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

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=JoinDb; Integrated Security=True;");
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
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Data seeding
/**

    * * Data seeding bir veritabanını ilk veri kümesiyle doldurma işlemidir.
    * * Seed Data'lar migration'ların dışında eklenmesi ve değiştirilmesi beklenmeyen durumlar için kullanılırlar.
    * * Data seeding genellikle şu durumlarda kullanışlıdır:
    
          1. Test için geçici verilere ihtiyaç varsa
          2. Asp.NET Core'daki Identity yapılanmasındaki roller gibi static değerler için
          3. Yazılım için temel konfigürasyonel değerler için


        - modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" });

    * * To add entities that have a relationship the foreign key values need to be specified:
        - modelBuilder.Entity<Post>().HasData(
              new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" });

*/
#endregion

#region adding seed data via HasData() method
// OnModelCreating() metodu içerisinde HasData() yöntemi ile tohum verileri ekleyebiliriz.
// Veritabanı tohumlanırken bir yönüyle ilişkisel verileri de üretebilmek adına Primary Key değerlerinin manuel olarak bildirilmesi gerekmektedir.
//    - modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" });
//    - modelBuilder.Entity<Post>().HasData(new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" });
#endregion

class Post
{
  public int Id { get; set; }
  public int BlogId { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }

  public Blog Blog { get; set; }
}
class Blog
{
  public int Id { get; set; }
  public string Url { get; set; }

  public ICollection<Post> Posts { get; set; }
}
class ApplicationDbContext : DbContext
{
  public DbSet<Post> Posts { get; set; }
  public DbSet<Blog> Blogs { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Post>()
      .HasData(
          new Post() { Id = 1, BlogId = 1, Title = "A", Content = "..." },
          new Post() { Id = 2, BlogId = 1, Title = "B", Content = "..." },
          new Post() { Id = 5, BlogId = 2, Title = "B", Content = "..." }
      );

    modelBuilder.Entity<Blog>()
        .HasData(
            new Blog() { Id = 11, Url = "www.gencayyildiz.com/blog" },
            new Blog() { Id = 2, Url = "www.bilmemne.com/blog" }
        );
  }
}
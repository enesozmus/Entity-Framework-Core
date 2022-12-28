using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

#region Data Adding Behaviors in Relational Scenarios

/** One-to-One **/
#region 1. Yöntem -> Principal Entity Üzerinden Dependent Entity Verisi Ekleme

/** DbContext Instance **/
Lesson20Context _context = new();

/** Via Object Reference **/
Student student1 = new();
student1.StudentName = "David";
student1.Address = new() { City = "London", Country = "England" };
await _context.AddAsync<Student>(student1);

/** Via Object Initializer **/
Student student2 = new()
{
  StudentName = "Kaisa",
  Address = new() { City = "Amsterdam", Country = "Holland" }
};
await _context.AddAsync<Student>(student2);
#endregion

#region 2. Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme

/** Via Object Initializer **/
StudentAddress studentAddress1 = new()
{
  City = "Istanbul",
  Country = "Turkey",
  Student = new() { StudentName = "Enes" }
};
await _context.AddAsync<StudentAddress>(studentAddress1);
#endregion


/** One-to-Many **/
#region 1. Yöntem -> Principal Entity Üzerinden Dependent Entity Verisi Ekleme
/** Via Object Reference **/
Blog blog1 = new();
blog1.BlogName = "Blog A";
blog1.Posts.Add(new() { Title = "Title A", Content = "Content A" });
blog1.Posts.Add(new() { Title = "Title B", Content = "Content C" });
blog1.Posts.Add(new() { Title = "Title C", Content = "Content C" });
await _context.AddAsync<Blog>(blog1);

/** Via Object Initializer **/
Blog blog2 = new()
{
  BlogName = "Blog B",
  Posts = new HashSet<Post> { new() { Title = "Title D", Content = "Content D" }, new() { Title = "Title E", Content = "Content E" } }
};
await _context.AddAsync<Blog>(blog2);
#endregion

#region 2. Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme
/** Via Object Initializer **/
// [tercih edilmez one-to-many mantığına aykırıdır]
Post post1 = new()
{
  Title = "Title G",
  Content = "Content G",
  Blog = new() { BlogName = "Blog G" }
};
await _context.AddAsync<Post>(post1);
#endregion

#region 3. Yöntem -> Foreign Key Kolonu Üzerinden Veri Ekleme
// 1. ve 2. yöntemler özünde veritabanında mevcut olmayan verilerin ilişkisel olarak eklenmesi durumunda kullanılılar.
// 3. yöntem ise veritabanında var olan bir Principal Entity verisiyle yeni Dependent Entity'lerin ilişkisel olarak eşlenmesi durumudur.
/** Via Object Initializer **/
Post post2 = new()
{
  BlogId = 7,
  Title = "Third Way",
  Content = "Third Way"
};
await _context.AddAsync<Post>(post2);
#endregion


/** Many-to-Many **/
#region 1. Yöntem -> Default Convention
// many-to-many ilişkisi 'Default Convention' üzerinden tasarlanmışsa kullanılan yöntemdir.
/** Via Object Initializer **/
// Book book = new()
// {
//   BookName = "A Kitabı",
//   Authors = new HashSet<Author>()
//   {
//     new(){ AuthorName = "Author A" },
//     new(){ AuthorName = "Author B" },
//     new(){ AuthorName = "Author C" },
//   }
// };
//await _context.Books.AddAsync(book);
#endregion

#region 2. Yöntem -> Fluent API
// many-to-many ilişkisi 'Fluent API' üzerinden tasarlanmışsa kullanılan yöntemdir.
/** Via Object Initializer **/
Author author = new()
{
  AuthorName = "Author Z",
  Books = new HashSet<AuthorBook>()
  {
    // veritabanında var olan
    new(){ BookId = 1},
    // veritabanında olmayan
    new(){ Book = new () { BookName = "B Kitap" } }
  }
};
#endregion

// await _context.SaveChangesAsync();


#region DbContext
public class Lesson20Context : DbContext
{
  public DbSet<Student> Student { get; set; }
  public DbSet<StudentAddress> StudentAddresses { get; set; }
  public DbSet<Blog> Blogs { get; set; }
  public DbSet<Post> Posts { get; set; }
  public DbSet<Book> Books { get; set; }
  public DbSet<Author> Authors { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=Lesson20Db; Integrated Security=True;");
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<AuthorBook>()
        .HasKey(x => new { x.AuthorId, x.BookId });

    modelBuilder.Entity<AuthorBook>()
        .HasOne(x => x.Book)
        .WithMany(x => x.Authors)
        .HasForeignKey(x => x.BookId);

    modelBuilder.Entity<AuthorBook>()
        .HasOne(x => x.Author)
        .WithMany(x => x.Books)
        .HasForeignKey(x => x.AuthorId);
    base.OnModelCreating(modelBuilder);
  }
}

/** one-to-one **/
public class Student
{
  public int StudentId { get; set; }
  public string StudentName { get; set; }

  public StudentAddress Address { get; set; }
}
public class StudentAddress
{
  [Key, ForeignKey("Student")]
  public int StudentAddressId { get; set; }
  public string City { get; set; }
  public string Country { get; set; }

  public Student Student { get; set; }
}

/** one-to-many **/
public class Blog
{
  public Blog()
  {
    // Blog üzerinden post eklerken null olmamasına ihtiyacımız var.
    // İlgili instance'ı burada üretiyoruz.
    Posts = new HashSet<Post>();
  }
  public int BlogId { get; set; }
  public string BlogName { get; set; }

  public ICollection<Post> Posts { get; set; }
}
public class Post
{
  public int PostId { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }

  public int BlogId { get; set; }
  public Blog Blog { get; set; }
}

/** many-to-many **/
public class Book
{
  public Book()
  {
    Authors = new HashSet<AuthorBook>();
    // Authors = new HashSet<Author>();
  }

  public int BookId { get; set; }
  public string BookName { get; set; }

  public ICollection<AuthorBook> Authors { get; set; }

  // public ICollection<Author> Authors { get; set; }
}
public class Author
{
  public Author()
  {
    Books = new HashSet<AuthorBook>();
    // Books = new HashSet<Book>();
  }
  public int AuthorId { get; set; }
  public string AuthorName { get; set; }

  public ICollection<AuthorBook> Books { get; set; }
  // public ICollection<Book> Books { get; set; }
}
/** Cross Table **/
public class AuthorBook
{
  public int BookId { get; set; }
  public int AuthorId { get; set; }

  public Book Book { get; set; }
  public Author Author { get; set; }
}
#endregion

#endregion
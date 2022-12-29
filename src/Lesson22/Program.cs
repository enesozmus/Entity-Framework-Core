using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

#region Data Deleting Behaviors in Relational Scenarios

/**

  * * Entity Framework Core, represents relationships 'using foreign keys'.
  * * An entity with a foreign key is the child or dependent entity in the relationship.
  * * This entity's foreign key value must match the primary key value of the related principal/parent entity.
  * * If the principal/parent entity is deleted, then the foreign key values of the dependents/children will no longer match the primary key of any principal/parent.
  * * There are two options to avoid this referential constraint violation:

        1. Set the FK values to null
            - The first option is only valid for optional relationships where the foreign key property (and the database column to which it is mapped) must be nullable.
        2. Also delete the dependent/child entities
            - The second option is valid for any kind of relationship and is known as "cascade delete".


  * * Consider this simple model where Blog is the principal/parent in a relationship with Post, which is the dependent/child.
  * * Post.BlogId is a foreign key property, the value of which must match the Blog.Id primary key of the blog to which the post belongs.
  * * By convention, this relationship is configured as a required, since the Post.BlogId foreign key property is non-nullable.
  * * Required relationships are configured to use cascade deletes by default.

  public class Blog
  {
      public int Id { get; set; }

      public string Name { get; set; }

      public IList<Post> Posts { get; } = new List<Post>();
  }
  public class Post
  {
      public int Id { get; set; }

      public string Title { get; set; }
      public string Content { get; set; }

      public int BlogId { get; set; }
      public Blog Blog { get; set; }
  }

  * ? Severing a relationship
  * * Rather than deleting the blog, we could instead sever the relationship between each post and its blog.
  * * This can be done by setting the reference navigation Post.Blog to null for each post:

*/

/** DbContext Instance **/
Lesson22Context _context = new();


#region Seed datas for the lesson
// Student student1 = new()
// {
//   StudentName = "David",
//   Address = new() { City = "London", Country = "England" }
// };
// Student student2 = new()
// {
//   StudentName = "Chelsea"
// };
// await _context.AddAsync<Student>(student1);
// await _context.Students.AddAsync(student2);

// Blog blog = new()
// {
//   BlogName = "Seed Blog",
//   Posts = new List<Post>
//     {
//         new(){ Title = "1. Post", Content = "1. Content" },
//         new(){ Title = "2. Post", Content = "2. Content" },
//         new(){ Title = "3. Post", Content = "3. Content" },
//     }
// };
// await _context.Blogs.AddAsync(blog);

// Book book1 = new() { BookName = "1. Kitap" };
// Book book2 = new() { BookName = "2. Kitap" };
// Book book3 = new() { BookName = "3. Kitap" };

// Author author1 = new() { AuthorName = "1. Yazar" };
// Author author2 = new() { AuthorName = "2. Yazar" };
// Author author3 = new() { AuthorName = "3. Yazar" };


// // book1.Authors.Add(author1);
// book1.Authors.Add(new() { Author = author1 });
// // book1.Authors.Add(author2);
// book1.Authors.Add(new() { Author = author2 });


// //book2.Authors.Add(author1);
// book2.Authors.Add(new() { Author = author1 });
// //book2.Authors.Add(author2);
// book2.Authors.Add(new() { Author = author2 });
// //book2.Authors.Add(author3);
// book2.Authors.Add(new() { Author = author3 });

// //book3.Authors.Add(author3);
// book3.Authors.Add(new() { Author = author3 });

// await _context.AddAsync(book1);
// await _context.AddAsync(book2);
// await _context.AddAsync(book3);
// await _context.SaveChangesAsync();
#endregion


#region One-to-One
// Student? student = await _context.Students
//                     .Include(x => x.Address)
//                     .FirstOrDefaultAsync(x => x.StudentId == 1);

// if (student != null)
//   _context.StudentAddresses.Remove(student.Address);
#endregion


#region One-to-Many
// Blog? blog = await _context.Blogs
//                  .Include(x => x.Posts)
//                  .FirstOrDefaultAsync(x => x.BlogId == 1);

// if (blog != null)
// {
//   Post? post = blog.Posts.FirstOrDefault(x => x.PostId == 3);
//   _context.Posts.Remove(post);
// }
#endregion


#region Many-to-Many
// → özünde cross table üzerinde silme işlemleri yapılır
Book? book = await _context.Books
                  .Include(x => x.Authors)
                  .FirstOrDefaultAsync(x => x.BookId == 1);
if (book != null)
{
  // bir cross table satırı
  AuthorBook? thatShouldBeDeleted = book.Authors.FirstOrDefault(a => a.AuthorId == 2);
  if (thatShouldBeDeleted != null)
  {
    // direkt yazarlara gidersen ilişkiyi değil, yazarı silersin!
    _context.AuthorBook.Remove(thatShouldBeDeleted);
    // _context.Remove(thatShouldBeDeleted);
  }
}
#endregion

#endregion

#region Cascade
//Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilerin silinmesini sağlar.
#endregion

#region SetNull
//Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilere null değerin atanmasını sağlar.

// Foreign key → nullable → ?
// IsRequired(false)
// .OnDelete(DeleteBehavior.SetNull)

//One to One senaryolarda eğer ki Foreign key ve Primary key kolonları aynı ise o zaman SetNull davranuışını KULLANAMAYIZ!
// ayrıca bir Foreign key kolonu kullanılırsa kullanılailir
#endregion

#region Restrict
//Esas tablodan herhangi bir veri silinmeye çalışıldığında o veriye karşılık dependent table'da ilişkisel veri/ler varsa eğer bu sefer bu silme işlemini engellemesini sağlar.
#endregion


/** DbContext **/
#region DbContext
public class Lesson22Context : DbContext
{
  public DbSet<Student> Students { get; set; }
  public DbSet<StudentAddress> StudentAddresses { get; set; }
  public DbSet<Blog> Blogs { get; set; }
  public DbSet<Post> Posts { get; set; }
  public DbSet<Book> Books { get; set; }
  public DbSet<Author> Authors { get; set; }
  public DbSet<AuthorBook> AuthorBook { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=Lesson22Db; Integrated Security=True;");
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
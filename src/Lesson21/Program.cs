using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

#region Data Modifying Behaviors in Relational Scenarios

/** DbContext Instance **/
Lesson21Context _context = new();


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


/** One-to-One **/
#region 1. Durum -> Principal Entity'deki veriye dependent veriyi değiştirme/güncelleme

// özünde → eski adresi silip yenisini ekleme
// Student? student = await _context.Students
//       .Include(x => x.Address)
//       .FirstOrDefaultAsync(x => x.StudentId == 1);

// if (student != null)
// {
//   _context.StudentAddresses.Remove(student.Address);
//   /** Via Object Initializer **/
//   student.Address = new() { City = "New Amsterdam", Country = "Holland" };
// }
#endregion

#region 2. Durum → Dependent verinin dependent olduğu ana veriyi değiştirme/güncelleme

// özünde → yanlış öğrenciye bağlanan eski adresi silip sildiğimiz adresi doğru öğrenciye bağlı yeni adres'miş gibi ekleme
// StudentAddress? address = await _context.StudentAddresses.FirstOrDefaultAsync(x => x.StudentAddressId == 1);
// if (address != null)
// {
//   _context.StudentAddresses.Remove(address);
//   Student? student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == 2);
//   if (student != null)
//   {
//     student.Address = new() { City = "New Amsterdam", Country = "Holland" };
//   }
// }
#endregion


/** One-to-Many **/
#region  1. Durum -> Principal Entity'deki veriye dependent veriyi değiştirme/güncelleme

// özünde → bloglar üzerinden istenmeyen postu silip yenilerini ekledeik
// Blog? blog = await _context.Blogs
//       .Include(x => x.Posts)
//       .FirstOrDefaultAsync(x => x.BlogId == 2);

// if (blog != null)
// {
//   Post? thatShouldBeDeleted = blog.Posts.FirstOrDefault(x => x.PostId == 2);
//   if (thatShouldBeDeleted != null)
//   {
//     blog.Posts.Remove(thatShouldBeDeleted);
//     blog.Posts.Add(new() { Title = "4. Post", Content = "4. Content" });
//     blog.Posts.Add(new() { Title = "5. Post", Content = "5. Content" });
//   }
// }
#endregion

#region 2. Durum → Dependent verinin dependent olduğu ana veriyi değiştirme/güncelleme

// özünde → yanlış bloğa bağlanan eski postu doğru bloğu bularak ya yeni blog ekleyerek ona bağlamak
// Post? post = await _context.Posts.FindAsync(4);
// if (post != null)
// {
//   Blog? blog = await _context.Blogs.FindAsync(3);
//   if (blog != null)
//   {
//     post.Blog = blog;
//   }
//   // post.Blog = new() { BlogName = "2. Blog" };
// }
#endregion


/** Many-to-Many **/
#region First Example
// özünde → bir kitabı başka bir yazarla daha ilişkilendirmek istiyoruz
// Book? book = await _context.Books.FirstOrDefaultAsync(x => x.BookId == 1);
// Author? author = await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId == 3);
// if (book != null && author != null)
// {
//   book.Authors.Add(new() { Author = author });
//   // book.Authors.Add(author);
// }
#endregion

#region Second Example
// özünde → yazara ait bazı kitapları ortadan kaldırarak ilişkileri de ortadan kaldırıyoruz
// Author? author = await _context.Authors
//                   .Include(a => a.Books)
//                   .FirstOrDefaultAsync(a => a.AuthorId == 3);

// if (author != null)
// {
//   foreach (AuthorBook? book in author.Books)
//   {
//     if (book.BookId != 1)
//       author.Books.Remove(book);
//   }
// }
#endregion

#region Third Example
// özünde → kitabın yazarlarla olan ilişkisini değiştirme
// Book? book = await _context.Books
//                 .Include(b => b.Authors)
//                 .FirstOrDefaultAsync(b => b.BookId == 2);

// if (book != null)
// {
//   AuthorBook? thatShouldBeDeleted = book.Authors.FirstOrDefault(x => x.AuthorId == 1);
//   //Author silinecekYazar = book.Authors.FirstOrDefault(a => a.Id == 1);

//   if (thatShouldBeDeleted != null)
//   {
//     book.Authors.Remove(thatShouldBeDeleted);
//     Author? author = await _context.Authors.FindAsync(3);
//     if (author != null)
//     {
//       book.Authors.Add(new() { Author = author });
//       book.Authors.Add(new() { Author = new() { AuthorName = "Last Author" } });
//     }
//   }
// }
#endregion
#endregion


/** DbContext **/
#region DbContext
public class Lesson21Context : DbContext
{
  public DbSet<Student> Students { get; set; }
  public DbSet<StudentAddress> StudentAddresses { get; set; }
  public DbSet<Blog> Blogs { get; set; }
  public DbSet<Post> Posts { get; set; }
  public DbSet<Book> Books { get; set; }
  public DbSet<Author> Authors { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=Lesson21Db; Integrated Security=True;");
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
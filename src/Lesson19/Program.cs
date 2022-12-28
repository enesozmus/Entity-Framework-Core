// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region Many-to-Many
#region Default Convention
/**

    * * Many-to-many relationships require a collection navigation property on both sides.
    * * Default Convention'da cross table'ı manuel oluşturmak zorunda değiliz.

    * * Composite Primary Key

*/

public class Post
{
  public int PostId { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }

  public ICollection<Tag> Tags { get; set; }
  //  public List<PostTag> PostTags { get; set; }
}

public class Tag
{
  public string TagId { get; set; }

  public ICollection<Post> Posts { get; set; }
  // public List<PostTag> PostTags { get; set; }
}

// Cross Table
public class PostTag
{
  public DateTime PublicationDate { get; set; }

  public int PostId { get; set; }
  public Post Post { get; set; }

  public string TagId { get; set; }
  public Tag Tag { get; set; }
}
#endregion


#region Data Annotations
/*

  * * Cross table manuel olarak oluşturulmak zorundadır.
  * * Entity'lerde oluşturduğumuz cross table entity'si ile bire çok bir ilişki kurulmalı.
  ? * Cross table'a karşılık bir entity modeli oluşturuyorsak eğer bunu context sınıfı içerisinde DbSet property'si olarka bildirmek mecburiyetinde değiliz!
  ! * CT'da composite primary key'i data annotation'lar ile manuel olarak kuramıyoruz. Bunun için de Fluent API'da çalışma yaopmamız gerekiyor.

      modelBuilder.Entity<BookAuthor>()
            .HasKey(x => new { x.BookId, x.AuthorId });

*/

public class Book
{
  public int BookId { get; set; }
  public string BookName { get; set; }

  public ICollection<BookAuthor> Authors { get; set; }
}

public class Author
{
  public int AuthorId { get; set; }
  public string AuthorName { get; set; }

  public ICollection<BookAuthor> Books { get; set; }
}

// Cross Table
public class BookAuthor
{
  [ForeignKey(nameof(Book))]
  public int BookId { get; set; }

  [ForeignKey(nameof(Author))]
  public int AuthorId { get; set; }

  public Book Book { get; set; }
  public Author Author { get; set; }
}
#endregion


#region Fluent API
/*

  * * Cross table manuel oluşturulmalı
  * * DbSet olarak eklenmesine lüzum yok
  * * Composite PK Haskey metodu ile kurulmalı!

      modelBuilder.Entity<BookAuthor>()
            .HasKey(x => new { x.BookId, x.AuthorId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ky => ky.Book)
            .WithMany(k => k.Authors)
            .HasForeignKey(ky => ky.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ky => ky.Author)
            .WithMany(y => y.Books)
            .HasForeignKey(ky => ky.AuthorId);
*/
public class Book
{
  public int BookId { get; set; }
  public string BookName { get; set; }

  public ICollection<BookAuthor> Authors { get; set; }
}
//Cross Table
class BookAuthor
{
  public int BookId { get; set; }
  public int AuthorId { get; set; }

  public Book Book { get; set; }
  public Author Author { get; set; }
}
public class Author
{
  public int AuthorId { get; set; }
  public string AuthorName { get; set; }

  public ICollection<BookAuthor> Books { get; set; }
}
#endregion
#endregion
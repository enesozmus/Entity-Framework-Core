// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region One-to-one
#region Default Convention
/*

  * * One to one relationships have a reference navigation property on both sides.
  * * In a One to one relationship PrimaryKey of the Primary table is both Primary key and Foreign key in the dependent table.
  * * → public int BlogId { get; set; }

  * * Tekil referans verecek şekilde her iki entity'de de navigation property'ler tanımlanır.
  * * One-to-one ilişki türünde, default olarak hangi entity'nin Dependent entity olduğunu EF Core bildirebilmek için...
  * * Dependent olacak entity'de fiziksel olarak bir foreign key'e karşılık bir property tanımlanır.
  * * → public int BlogId { get; set; }

*/

// Principal Entity
public class Blog
{
  public int BlogId { get; set; }
  public string Url { get; set; }

  public BlogImage BlogImage { get; set; }
}

// Dependent Entity
public class BlogImage
{
  public int BlogImageId { get; set; }
  public byte[] Image { get; set; }
  public string Caption { get; set; }

  public int BlogId { get; set; }
  public Blog Blog { get; set; }
}
#endregion


#region Data Annotations
/*

  * * One to one relationships have a reference navigation property on both sides.

  * * Tekil referans verecek şekilde her iki entity'de de navigation property'ler tanımlanır.
  * * One-to-one ilişki türünde, Data Annotations kullanılarak hangi entity'nin Principal/Primary hangi entity'nin Dependent entity olduğunu bildirebiliriz.
  * * [Key, ForeignKey("Blog")]

*/

// Principal Entity
public class Blog
{
  public int BlogId { get; set; }
  public string Url { get; set; }

  public BlogImage BlogImage { get; set; }
}

// Dependent Entity
public class BlogImage
{
  [Key, ForeignKey("Blog")]
  // [Key, ForeignKey(nameof(Blog))]
  public int BlogImageId { get; set; }
  public byte[] Image { get; set; }
  public string Caption { get; set; }

  public Blog Blog { get; set; }
}
#endregion


#region Fluent API
/*

  * * One to one relationships have a reference navigation property on both sides.

  * * Tekil referans verecek şekilde her iki entity'de de navigation property'ler tanımlanır.
  * * Fleunt API yönteminde entity'ler arasındaki ilişki DbContext sınıfı içerisindeki OnModelCreating fonksiyonu override edilerek tasarlanır.
  ? ? protected override void OnModelCreating(ModelBuilder modelBuilder) { }


  * * The Fluent API Provides four methods to define the navigational properties

        1. HasOne()
        2. HasMany()
        3. WithOne()
        4. WithMany()

        - We always begin with HasOne() or HasMany() on the entity on which you are configuring.
        - We then chain it with WithOne() WithMany() to configure the other side of the relationship

*/

// Principal Entity
public class Blog
{
  public int BlogId { get; set; }
  public string Url { get; set; }

  public BlogImage BlogImage { get; set; }
}

// Dependent Entity
public class BlogImage
{
  public int BlogImageId { get; set; }
  public byte[] Image { get; set; }
  public string Caption { get; set; }

  // Since, we do not have Primary Key defined on [BlogImage] class, let us define it first using HasKey method.
  public Blog Blog { get; set; }
}

/*

  ? Since, we do not have Primary Key defined on [BlogImage] class, let us define it first using HasKey method.

      modelBuilder.Entity<BlogImage>()
                .HasKey(s => s.BlogImageId);

  ? Next, we start by configuring the [Blog] entity.

      modelBuilder.Entity<Blog>()
  
  ? Since the [Blog] class is belongs to “One” side of the relation, we will need HasOne method.

      modelBuilder.Entity<Blog>()
                .HasOne<BlogImage>(p => p.BlogImage)

  ? Finally, we move one to the other end of the relationship (i.e BlogImage), which is also “one” side of the relation.

      modelBuilder.Entity<BlogImage>()
                .HasKey(s => s.BlogImageId);

      modelBuilder.Entity<Blog>()
          .HasOne<BlogImage>(p => p.BlogImage)
          .WithOne(s => s.Blog)
          .HasForeignKey<BlogImage>(s => s.BlogImageId);

  ? Since, we have Primary Key defined on [BlogImage] class,

      modelBuilder.Entity<BlogImage>()
           .HasKey(s => s.BlogImageId);
 
      modelBuilder.Entity<Blog>()
                  .HasOne<BlogImage>(p => p.BlogImage)
                  .WithOne(s => s.Blog)
                  .HasForeignKey<BlogImage>(s => s.BlogId);

*/
#endregion
#endregion
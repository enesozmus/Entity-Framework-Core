// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


#region Basic_Concepts_of_Relational_Database
#region Principal Entity & Dependent Entity
/*

    * * The entity that will serve as a 'parent' in a relationship is termed a principal entity.
    * * This is the entity that contains the primary key properties. 
    * * Kendi başına var olabilen bir tabloyu modelleyen entity'e denir.

    * * The entity that will serve as a 'child' in a relationship is termed a principal entity.
    * * This is the entity that contains the foreign key properties.
    * * Kendi başına var olamayan başka bir tabloya ilişkisel olarak bağımlı olan tabloyu modelleyen entity'e denir.

*/
#endregion


#region Principal Key & Foreign Key
/*

    * * The properties that uniquely identify the principal entity.
    * * Principal Entity'deki ID'nin kendisidir. Principal Entity'nin kimliği olan kolonu ifade eden propertydir.

    * * The properties in the dependent entity that are used to store the principal key values for the related entity.
    * * Principal Entity ile Dependent Entity arasındaki ilişkiyi sağlayan key'dir.
    * * Dependent Entity'de tanımlanır.
    * * Principal Entity'deki Principal Key'i tutar.

*/
#endregion


#region Navigation Property
/*

    * * Navigation properties provide a way to navigate an association between two entity types.
    * * Every object can have a navigation property for every relationship in which it participates.
    * * A property defined on the principal and/or dependent entity that references the related entity.
    * * Navigation properties allow you to navigate and manage relationships in both directions, returning either a reference object (if the multiplicity is either one or zero-or-one) or a collection (if the multiplicity is many).

        1. Collection navigation property: A navigation property that contains references to many related entities.
        2. Reference navigation property: A navigation property that holds a reference to a single related entity.
        3. Inverse navigation property: When discussing a particular navigation property, this term refers to the navigation property on the other end of the relationship.
   
    * * İlişkisel tablolar arasındaki fiziksel erişimi entity'ler üzerinden sağlayan property'lerdir.
    * * Bir property'nin navigation property olabilmesi için kesinlikle entity türünden olması gerekir.

    * * By default, a relationship will be created when there is a navigation property discovered on a type.
    * * A property is considered a navigation property if the type it points to cannot be mapped as a scalar type by the current database provider.

*/
#endregion


#region Relationship Patterns
#region One-to-one
/*

    * * One to one relationships have a reference navigation property on both sides.
    * * They follow the same conventions as one-to-many relationships, but a unique index is introduced on the foreign key property to ensure only one dependent is related to each principal.

    * * When configuring the relationship with the Fluent API, you use the HasOne and WithOne methods.
    * * When configuring the foreign key you need to specify the dependent entity type - notice the generic parameter provided to HasForeignKey in the listing below.
    * * In a one-to-many relationship it is clear that the entity with the reference navigation is the dependent and the one with the collection is the principal. But this is not so in a one-to-one relationship - hence the need to explicitly define it.

          modelBuilder.Entity<Blog>()
            .HasOne(b => b.BlogImage)
            .WithOne(i => i.Blog)
            .HasForeignKey<BlogImage>(b => b.BlogForeignKey);

*/
public class Blog
{
  public int BlogId { get; set; }
  public string Url { get; set; }

  public BlogImage BlogImage { get; set; }
}
public class BlogImage
{
  public int BlogImageId { get; set; }
  public byte[] Image { get; set; }
  public string Caption { get; set; }

  public int BlogId { get; set; }
  public Blog Blog { get; set; }
}
#endregion

#region Many-to-many
/*

  * * Many-to-many relationships require a collection navigation property on both sides.
  * * The way this relationship is implemented in the database is by a join table that contains foreign keys to both Post and Tag. 
  
  * * It is common to apply configuration to the join entity type. This action can be accomplished via UsingEntity.

        modelBuilder
          .Entity<Post>()
          .HasMany(p => p.Tags)
          .WithMany(p => p.Posts)
          .UsingEntity(j => j.ToTable("PostTags"));

  * * Seed datas

        modelBuilder
            .Entity<Post>()
            .HasData(new Post { PostId = 1, Title = "First" });

        modelBuilder
            .Entity<Tag>()
            .HasData(new Tag { TagId = "ef" });

        modelBuilder
            .Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(p => p.Posts)
            .UsingEntity(j => j.HasData(new { PostsPostId = 1, TagsTagId = "ef" }));
*/
public class Post
{
  public int PostId { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }

  public ICollection<Tag> Tags { get; set; }
  public List<PostTag> PostTags { get; set; }
}
public class Tag
{
  public string TagId { get; set; }

  public ICollection<Post> Posts { get; set; }
  public List<PostTag> PostTags { get; set; }
}
public class PostTag
{
  public DateTime PublicationDate { get; set; }

  public int PostId { get; set; }
  public Post Post { get; set; }

  public string TagId { get; set; }
  public Tag Tag { get; set; }
}
/*
        modelBuilder.Entity<PostTag>()
            .HasKey(t => new { t.PostId, t.TagId });

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId);

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public List<PostTag> PostTags { get; set; }
}

public class Tag
{
    public string TagId { get; set; }

    public List<PostTag> PostTags { get; set; }
}

public class PostTag
{
    public DateTime PublicationDate { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }

    public string TagId { get; set; }
    public Tag Tag { get; set; }
}
*/
/*
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(p => p.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagsId")
                    .HasConstraintName("FK_PostTag_Tags_TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostsId")
                    .HasConstraintName("FK_PostTag_Posts_PostId")
                    .OnDelete(DeleteBehavior.ClientCascade));
*/
#endregion

#region One-to-many
// public class Blog
// {
//   public int BlogId { get; set; }
//   public string Url { get; set; }

//   public List<Post> Posts { get; set; }
// }

// public class Post
// {
//   public int PostId { get; set; }
//   public string Title { get; set; }
//   public string Content { get; set; }

//   public int BlogId { get; set; }
//   public Blog Blog { get; set; }
// }
#endregion
#endregion


#region Ways to Configure Relationships
#region Default Conventions
// Varsayılan entity kurallarını kullanarak yapılan ilişki yapılandırma yöntemleridir.
// Navigation property'leri kullanarak ilişki şablonlarını çıkarmaktadır.
#endregion


#region Data Annotations Attributes
// Entity'nin niteliklerine göre ince ayarlar yapmamızı sağlayan attribute'lardır. [Key], [ForeignKey]
#endregion


#region Fluent API
/**
  * * To configure a relationship in the Fluent API, you start by identifying the navigation properties that make up the relationship.
  * * Entity modellerindeki ilişkileri yapılandırırken daha detaylı çalışmamızı sağlayan bir yoldur.
*/
#region HasOne
// İlgili entity'nin ilişkisel entity'ye birebir ya da bire çok olacak şekilde ilişkisini yapılandırmaya başlayan metottur.
#endregion

#region HasMany
// İlgili entity'nin ilişkisel entity'ye çoka bir ya da çoka çok olacak şekilde ilişkisini yapıulandırmaya başlayan metottur.
#endregion

#region WithOne
// HasOne ya da HasMany'den sonra bire bir ya da çoka bir olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion

#region WithMany
// HasOne ya da HasMany'den sonra bire çok ya da çoka çok olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion
#endregion
#endregion
#endregion


/**

  * * Blog is the principal entity

  * * Blog.BlogId is the principal key

  * * Blog.Posts is a collection navigation property


  * * Post is the dependent entity

  * * Post.BlogId is the foreign key

  * * Post.Blog is a reference navigation property
  
  * ? Post.Blog is the inverse navigation property of Blog.Posts (and vice versa)

  // public class Blog
  // {
  //   public int BlogId { get; set; }
  //   public string Url { get; set; }

  //   public List<Post> Posts { get; set; }
  // }

  // public class Post
  // {
  //   public int PostId { get; set; }
  //   public string Title { get; set; }
  //   public string Content { get; set; }

  //   public int BlogId { get; set; }
  //   public Blog Blog { get; set; }
  // }

*/
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region One-to-Many
#region Default Convention
// Principal Entity
public class Blog
{
  public int BlogId { get; set; }
  public string Url { get; set; }

  public List<Post> Posts { get; set; }
}

// Dependent Entity
public class Post
{
  public int PostId { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }

  public int BlogId { get; set; }
  public Blog Blog { get; set; }
}
#endregion


#region Data Annotations
// Default convention yönteminde foreign key kolonuna karşılık gelen property'i tanımladığımızda bu property ismi EF Core entity tanımlama kurallarına uymuyorsa Data Annotations'lar ile müdahalede bulunabiliriz.
// → [ForeignKey(nameof(Blog))]
#endregion


#region Fluent API
/*
    modelBuilder.Entity<Post>()
        .HasOne(c => c.Blog)
        .WithMany(d => d.Posts)
        .HasForeignKey(c => c.BlogId);
        .IsRequired(false); → nullable column
        .IsRequired(true);
*/
#endregion
#endregion
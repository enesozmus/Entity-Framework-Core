Console.WriteLine("Hello, World!");

#region Keyless Entity Types
/**

  * ! Keyless entity types can be used to carry out database queries against data that doesn't contain key values.
  * ! However, they are different from regular entity types in that they:
    
      1. Cannot have a key defined.
      2. They may never act as the principal end of a relationship.
      3. Are never tracked for changes in the DbContext and therefore are never inserted, updated or deleted on the database.
      4. TPH olarak entity hiyerarşisinde kullanılabilir lakin diğer kalıtımsal ilişkilerde kullanılamaz!
  
  * ! Some of the main usage scenarios for keyless entity types are:

      1. Serving as the return type for SQL queries.
      2. Mapping to database views that do not contain a primary key.
      3. Mapping to tables that do not have a primary key defined.
      4. Mapping to queries defined in the model

  * ! Keyless entity types can be defined as follows:
  * * Bu entity'nin ayrıca DbSet<> property'si olarak DbContext nesnesine eklenmesi gerekmektedir.

    - Data Annotations

        [Keyless]
        public class BlogPostsCount
        {
            public string BlogName { get; set; }
            public int PostCount { get; set; }
        }

    - Fluent API
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPostsCount>()
                .HasNoKey();
        }
*/
#endregion
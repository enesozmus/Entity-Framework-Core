/** Database cascade limitations

  * * Some databases, most notably SQL Server, have limitations on the cascade behaviors that form cycles.
  * * This model has three relationships, all required and therefore configured to cascade delete by convention:

        1. three relationships
        2. all required
        3. configured to cascade delete
    
    - Deleting a blog will cascade delete all the related posts
    - Deleting the author of posts will cause the authored posts to be cascade deleted
    - Deleting the owner of a blog will cause the blog to be cascade deleted

  * * This is all reasonable but attempting to create a SQL Server database with these cascades configured results in the following exception:

    Microsoft.Data.SqlClient.SqlException (0x80131904): Introducing FOREIGN KEY constraint 'FK_Posts_Person_AuthorId' on table 'Posts' may cause cycles or multiple cascade paths.
    Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.

  * * There are two ways to handle this situation:

        1. Change one or more of the relationships to not cascade delete.

            - Taking the first approach with our example, we could make the blog-owner relationship optional by giving it a nullable foreign key property:
            - public int? BlogId { get; set; }
            - An optional relationship allows the blog to exist without an owner, which means cascade delete will no longer be configured by default.
            - This means there is no longer a cycle in cascading actions, and the database can be created without error on SQL Server.

        2. Configure the database without one or more of these cascade deletes
           then ensure all dependent entities are loaded so that EF Core can perform the cascading behavior.

           - Taking the second approach instead, we can keep the blog-owner relationship required and configured for cascade delete,
           - but make this configuration only apply to tracked entities, not the database:

               modelBuilder
                .Entity<Blog>()
                .HasOne(e => e.Owner)
                .WithOne(e => e.OwnedBlog)
                .OnDelete(DeleteBehavior.ClientCascade);

           - Now what happens if we load both a person and the blog they own, then delete the person?
           - EF Core will cascade the delete of the owner so that the blog is also deleted:

                using var context = new BlogsContext();

                var owner = context.People.Single(e => e.Name == "ajcvickers");
                var blog = context.Blogs.Single(e => e.Owner == owner);

                context.Remove(owner);

                context.SaveChanges();

          - However, if the blog is not loaded when the owner is deleted:

                using var context = new BlogsContext();

                var owner = context.People.Single(e => e.Name == "ajcvickers");

                context.Remove(owner);

                context.SaveChanges();

          ! Then an exception will be thrown due to violation of the foreign key constraint in the database:
*/

/**

  * * Optional relationships have nullable foreign key properties mapped to nullable database columns.
  * * This means that the foreign key value can be set to null when the current principal/parent is deleted or is severed from the dependent/child.
  * * public int? BlogId { get; set; }
  * ↑ This foreign key property will be set to null for each post when its related blog is deleted. 

      using var context = new BlogsContext();

      var blog = context.Blogs.OrderBy(e => e.Name).Include(e => e.Posts).First();

      context.Remove(blog);

      context.SaveChanges();
*/

#region Entities
// public class Blog
// {
//   public int Id { get; set; }
//   public string Name { get; set; }

//   public IList<Post> Posts { get; } = new List<Post>();

//   public int OwnerId { get; set; }
//   public Person Owner { get; set; }
// }

// public class Post
// {
//   public int Id { get; set; }
//   public string Title { get; set; }
//   public string Content { get; set; }

//   public int BlogId { get; set; }
//   public Blog Blog { get; set; }

//   public int AuthorId { get; set; }
//   public Person Author { get; set; }
// }

// public class Person
// {
//   public int Id { get; set; }
//   public string Name { get; set; }

//   public IList<Post> Posts { get; } = new List<Post>();

//   public Blog OwnedBlog { get; set; }
// }
#endregion
Console.WriteLine("Hello, World!");

#region Global Query Filters Nedir?
/**

  * ! Global query filters are LINQ query predicates applied to Entity Types in the metadata model (usually in OnModelCreating).
  * ! A query predicate is a boolean expression typically passed to the LINQ Where query operator.
  * ! EF Core applies such filters automatically to any LINQ queries involving those Entity Types.
  * ! EF Core also applies them to Entity Types, referenced indirectly through use of Include or navigation property.
  * ? Filters can only be defined for the root Entity Type of an inheritance hierarchy.
  * ! Some common applications of this feature are:

    1. Soft delete - An Entity Type defines an IsDeleted property.
    2. Multi-tenancy - An Entity Type defines a TenantId property.

  * * Bir entity'e özel, uygulama seviyesinde ön kabullü şartlar oluşturmamızı ve böylece verileri global bir şekilde filtreleyebilmemizi sağlayan bir özelliktir.
  * * Böylece belirtilen entity üzerinden yapılan tüm sorgulamalarda ekstradan bir şart ifadesine gerek kalmaksızın filtreleri otomatik uygulayarak hızlıca sorgulama yapabilmekteyiz.
*/
#region Example - 1
/**

  * * Note the declaration of a '_tenantId' field on the Blog entity.
  * * This field will be used to associate each Blog instance with a specific tenant.

  * * Also defined is an IsDeleted property on the Post entity type.
  * * This property is used to keep track of whether a post instance has been "soft-deleted".
  * * That is, the instance is marked as deleted without physically removing the underlying data.

  public class Blog
  {
    private string _tenantId;
    public int BlogId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public List<Post> Posts { get; set; }
  }
  public class Post
  {
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsDeleted { get; set; }

    public Blog Blog { get; set; }
  }

  * ! Next, configure the query filters in OnModelCreating using the HasQueryFilter() API.
  * ? The predicate expressions passed to the HasQueryFilter() calls will now automatically be applied to any LINQ queries for those types:

    - modelBuilder.Entity<Blog>().HasQueryFilter(b => EF.Property<string>(b, "_tenantId") == _tenantId);
    - modelBuilder.Entity<Post>().HasQueryFilter(p => !p.IsDeleted);
*/
#endregion


#region Example - 2
/**

  public class Person
  {
      public int PersonId { get; set; }
      public string Name { get; set; }
      public bool IsActive { get; set; }

      public List<Order> Orders { get; set; }
  }

  * ! Next, configure the query filters in OnModelCreating using the HasQueryFilter() API.
  * ? The predicate expressions passed to the HasQueryFilter() calls will now automatically be applied to any LINQ queries for those types:

    - modelBuilder.Entity<Person>().HasQueryFilter(p => p.IsActive);

  * *

    - nope → await _context.Persons.Where(p => p.IsActive).ToListAsync();
    - yes  → await _context.Persons.ToListAsync();
    - yes  → await _context.Persons.FirstOrDefaultAsync(p => p.Name.Contains("a") || p.PersonId == 3);
*/
#endregion


#region IgnoreQueryFilters() - Disabling Filters
/**

  * * Filters may be disabled for individual LINQ queries by using the IgnoreQueryFilters operator.

    - blogs = db.Blogs.Include(b => b.Posts).IgnoreQueryFilters().ToList();
    - await _context.Persons.IgnoreQueryFilters().ToListAsync();
*/
#endregion


#region Use of navigations
/**

  * * You can also use navigations in defining global query filters.
  * * Using navigations in query filter will cause query filters to be applied recursively.
  * * When EF Core expands navigations used in query filters, it will also apply query filters defined on referenced entities.

  * ! To illustrate this configure query filters in OnModelCreating in the following way:

    - modelBuilder.Entity<Blog>().HasMany(b => b.Posts).WithOne(p => p.Blog);
    - modelBuilder.Entity<Blog>().HasQueryFilter(b => b.Posts.Count > 0);
    - modelBuilder.Entity<Post>().HasQueryFilter(p => p.Title.Contains("fish"));

  * * Next, query for all Blog entities:

    - var filteredBlogs = db.Blogs.ToList();

  * * This query produces the following SQL, which applies query filters defined for both Blog and Post entities:

    SELECT [b].[BlogId], [b].[Name], [b].[Url]
    FROM [Blogs] AS [b]
    WHERE (
        SELECT COUNT(*)
        FROM [Posts] AS [p]
        WHERE ([p].[Title] LIKE N'%fish%') AND ([b].[BlogId] = [p].[BlogId])) > 0

  * ! Currently EF Core does not detect cycles in global query filter definitions, so you should be careful when defining them.
  * ! If specified incorrectly, cycles could lead to infinite loops during query translation.

*/
#region Example - 1
/**

  public class Person
  {
    public int PersonId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public List<Order> Orders { get; set; }
  }
  public class Order
  {
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public Person Person { get; set; }
  }
    - modelBuilder.Entity<Person>().HasQueryFilter(p => p.Orders.Count > 0);
    - nope → await _context.Persons.AsNoTracking().Include(p => p.Orders).Where(p => p.Orders.Count > 0).ToListAsync();
    - yes  → await _context.Persons.AsNoTracking().ToListAsync();
*/
#endregion
#endregion
#endregion
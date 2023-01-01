Console.WriteLine("Hello, World!");

#region Loading Related Data
/**

    * * Entity Framework Core allows you to use the navigation properties in your model to load related entities.
    * * There are three common O/RM patterns used to load related data:

    1. Eager loading means that the related data is loaded from the database as part of the initial query.
    2. Explicit loading means that the related data is explicitly loaded from the database at a later time.
    3. Lazy loading means that the related data is transparently loaded from the database when the navigation property is accessed.
*/
#endregion

#region Eager loading
/**

    * * You can use the 'Include()' method to specify related data to be included in query results.
    * ? 'Include()': Eager loading operasyonunu yapmamızı sağlayan bir fonksiyondur.
    * ? Arka planda üretilen sorguya Join ekler.
    * ? Eager loading, generate edilen bir sorguya ilişkisel verilerin parça parça eklenmesini sağlayan ve bunu yaparken iradeli/istekli bir şekilde yapabilmemizi sağlayan bir yöntemdir.
    * * In the following example, the blogs that are returned in the results will have their Posts property populated with the related posts.

    var blogs = context.Blogs
        .Include(blog => blog.Posts)
        .ToList();

    var employees = context.Orders
        .Include(e => e.Employee.Region)        // → tekil referanslı senaryolar için
        .ToList();

    * * You can include related data from multiple relationships in a single query.
    * ! Eager loading a collection navigation in a single query may cause performance issues.

    var blogs = context.Blogs
        .Include(blog => blog.Posts)
        .Include(blog => blog.Owner)
        .ToList();
*/
#endregion

#region Including multiple levels
/**

    * * You can drill down through relationships to include multiple levels of related data using the 'ThenInclude()' method.
    * ? 'ThenInclude()', üretilen sorguda Include edilen tabloların ilişkili olduğu diğer tablolarıda sorguya ekleyebilmek için kullanılan bir fonksiyondur.
    * ? Üretilen sorguya 'Include()' edilen Navigation Property tekil değil, koleksiyonel bir property ise bu property üzerinden diğer ilişkisel tabloya erişim gösterilememektedir.
    * ? Böyle bir durumda koleksiyonel property'lerin türlerine erişip, o tür ile ilişkili diğer tabloları da sorguya eklememizi sağlayan fonksiyondur
    * * The following example loads all blogs, their related posts, and the author of each post.

        var blogs = context.Blogs
            .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
            .ToList();

    * * You can chain multiple calls to ThenInclude to continue including further levels of related data.

        var blogs = context.Blogs
            .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
            .ThenInclude(author => author.Photo)
            .ToList();

    * * Finally

        var blogs = context.Blogs
            .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
            .ThenInclude(author => author.Photo)
            .Include(blog => blog.Owner)
            .ThenInclude(owner => owner.Photo)
            .ToList();

        var blogs = context.Blogs
            .Include(blog => blog.Owner.AuthoredPosts)
            .ThenInclude(post => post.Blog.Owner.Photo)
            .ToList();
*/
#endregion

#region Filtered include
/**

    * * When applying Include to load related data, you can add certain enumerable operations to the included collection navigation, which allows for filtering and sorting of the results.
    * ? Sorgulama süreçlerinde 'Include()' yaparken sonuçlar üzerinde filtreleme ve sıralama gerçekleştirebilmemiz isağlayan bir özleliktir.
    * ? Supported operations are: Where, OrderBy, OrderByDescending, ThenBy, ThenByDescending, Skip, and Take.
    * * Such operations should be applied on the collection navigation in the lambda passed to the Include method, as shown in example below:

    var regions = context.Regions
        .Include(r => r.Employees.Where(e => e.Name.Contains("a")).OrderByDescending(e => e.Surname))
        .ToList();

    var filteredBlogs = context.Blogs
        .Include(
            blog => blog.Posts
                .Where(post => post.BlogId == 1)
                .OrderByDescending(post => post.Title)
                .Take(5))
        .ToList();

    * * Not: Change Tracker'ın aktif olduğu durumlarda Include ewilmiş sorgular üzerindeki filtreleme sonuçları beklenmedik şekilde olabilir.
    Bu durum, daha önce sorgulanmş ve Change Tracker tarafından takip edilmiş veriler arasında filtrenin gereksinimi dışında kalan veriler için söz konusu olacaktır.
    Bundan dolayı sağlıklı bir filtred include operasyonu için change tracker'ın kullanılmadığı sorguları tercih etmeyi düşünebilirsiniz.

*/
#endregion

#region Note
// EF Core, önceden üretilmiş ve execute edilerek verileri belleğe alınmış olan sorguların verileri, sonraki sorgularda KULLANIR!
/*
        var orders = await context.Orders.ToListAsync();
        var employees = await context.Employees.ToListAsync();

        employee'lara bakıldığında 'Include()' yapılmış gibi order'ların da gelmiş olduğu görülür.
        bu senaryoda ayrıca bir 'Include()' kullanmak ekstradan bir maliyet demektir.
*/
#endregion

#region AutoInclude - EF Core 6
// Uygulama seviyesinde bir entitye karşılık yapılan tüm sorgulamalarda "kesinlikle" bir tabloya Include işlemi gerçekleştirlecekse bunu her bir sorgu için tek tek yapmaktansa
// ...bunu merkezi bir hale getirmemizi sağlayan özelliktir.

// modelBuilder.Entity<Employee>()
//     .Navigation(e => e.Region)
//     .AutoInclude();
// var employees = await context.Employees.ToListAsync();
#endregion

#region IgnoreAutoIncludes
// AutoInclude konfigürasyonunu sorgu seviyesinde pasifize edebilmek için kullandığımız fonksiyondur.
// var employees = await context.Employees.IgnoreAutoIncludes().ToListAsync();
#endregion

#region Include on derived types
/**

    * * You can include related data from navigation defined only on a derived type using Include and ThenInclude.
    * * Contents of School navigation of all People who are 'Students' can be eagerly loaded using many patterns:

        1. Using cast
            - context.People.Include(person => ((Student)person).School).ToList()
            - var persons1 = await context.Persons.Include(p => ((Employee)p).Orders).ToListAsync();
        2. Using as operator
            - context.People.Include(person => (person as Student).School).ToList()
            - var persons2 = await context.Persons.Include(p => (p as Employee).Orders).ToListAsync();
        3. Using overload of Include that takes parameter of type string
            - context.People.Include("School").ToList()
            - var persons3 = await context.Persons.Include("Orders").ToListAsync();

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class Student : Person
{
    public School School { get; set; }
}
public class School
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Student> Students { get; set; }
}
public class SchoolContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<School> Schools { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<School>().HasMany(s => s.Students).WithOne(s => s.School);
    }
}
*/
#endregion
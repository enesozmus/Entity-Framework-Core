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

#region Lazy Loading
/**

    * * Lazy loading is delaying the loading of related data, until you specifically request for it.
    * * It is the opposite of eager loading.
    
    * * For example, the Student entity contains the StudentAddress entity.
    * * In the lazy loading, the context first loads the Student entity data from the database,
    * * ...then it will load the StudentAddress entity when we access the StudentAddress property as shown below:

        using (var ctx = new SchoolDBEntities())
        {
            //Loading students only
            IList<Student> studList = ctx.Students.ToList<Student>();

            Student std = studList[0];

            //Loads Student address for particular Student only (seperate SQL query)
            StudentAddress add = std.StudentAddress;
        }
    
    * ? Navigation property'ler üzerinde bir işlem yapılmaya çalışıldığı taktirde ilgili propertynin/ye temsil ettiği/karşılık gelen tabloya özel bir sorgu oluşturulup execute edilmesini ve verilerin yüklenmesini sağlayan bir yaklaşımdır.
*/
#endregion

#region Lazy loading with proxies
/**

    * * The simplest way to use lazy-loading is by installing the Microsoft.EntityFrameworkCore.Proxies package
    * * ...and enabling it with a call to UseLazyLoadingProxies. For example:
    * ?  Proxy'ler üzerinden lazy loading operasyonu gerçekleştiriyorsanız Navigtation Property'lerin 'virtual' ile işaretlenmiş olması gerekmektedir.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(myConnectionString);
        }

        .AddDbContext<BloggingContext>(
            b => b.UseLazyLoadingProxies()
                .UseSqlServer(myConnectionString));

    * * EF Core will then enable lazy loading for any navigation property that can be overridden--that is, it must be virtual and on a class that can be inherited from.
    * * For example, in the following entities, the Post.Blog and Blog.Posts navigation properties will be lazy-loaded.

        public class Blog
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public virtual ICollection<Post> Posts { get; set; }
        }

        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public virtual Blog Blog { get; set; }
        }
*/
#endregion

#region Lazy loading without proxies
/**

    * ? Proxy'ler tüm platformlarda desteklenmeyebilir. Böyle bir durumda manuel bir şekilde lazy loading uygulamak mecburiyetinde kalabiliriz.
    * ! Manuel yapılan Lazy Loading operasyonlarında Navigation Proeprty'lerin 'virtual' ile işaretlenmesine gerek yoktur!
    * * Lazy-loading without proxies work by injecting the ILazyLoader service into an entity, as described in Entity Type Constructors.
    * * For example:

        public class Blog
        {
            private ICollection<Post> _posts;

            public Blog()
            {
            }

            private Blog(ILazyLoader lazyLoader)
            {
                LazyLoader = lazyLoader;
            }

            private ILazyLoader LazyLoader { get; set; }

            public int Id { get; set; }
            public string Name { get; set; }

            public ICollection<Post> Posts
            {
                get => LazyLoader.Load(this, ref _posts);
                set => _posts = value;
            }
        }

        public class Post
        {
            private Blog _blog;

            public Post()
            {
            }

            private Post(ILazyLoader lazyLoader)
            {
                LazyLoader = lazyLoader;
            }

            private ILazyLoader LazyLoader { get; set; }

            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public Blog Blog
            {
                get => LazyLoader.Load(this, ref _blog);
                set => _blog = value;
            }
        }

*/

#region ILazyLoader Interface'i İle Lazy Loading
// Microsoft.EntityFrameworkCore.Abstractions

//public class Employee
//{
//    ILazyLoader _lazyLoader;
//    Region _region;
//    public Employee() { }
//    public Employee(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public List<Order> Orders { get; set; }
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);
//        set => _region = value;
//    }
//}

#endregion
#region Delegate İle Lazy Loading
// Herhangi bir hazır kütüphane kullanmaksızın...
//public class Employee
//{
//    Action<object, string> _lazyLoader;
//    Region _region;
//    public Employee() { }
//    public Employee(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public List<Order> Orders { get; set; }
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);
//        set => _region = value;
//    }
//}
//public class Region
//{
//    ILazyLoader _lazyLoader;
//    ICollection<Employee> _employees;
//    public Region() { }
//    public Region(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
//}
//public class Region
//{
//    Action<object, string> _lazyLoader;
//    ICollection<Employee> _employees;
//    public Region() { }
//    public Region(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
//}

//static class LazyLoadingExtension
//{
//    public static TRelated Load<TRelated>(this Action<object, string> loader, object entity, ref TRelated navigation, [CallerMemberName] string navigationName = null)
//    {
//        loader.Invoke(entity, navigationName);
//        return navigation;
//    }
//}
// Or
// public static class PocoLoadingExtensions
// {
//     public static TRelated Load<TRelated>(
//         this Action<object, string> loader,
//         object entity,
//         ref TRelated navigationField,
//         [CallerMemberName] string navigationName = null)
//         where TRelated : class
//     {
//         loader?.Invoke(entity, navigationName);

//         return navigationField;
//     }
// }
#endregion
#endregion

#region N+1 Problemi
// ! Hangi Lazy Loading yöntemi uygulanırsa uygulansın 'n+1' adını verdiğimiz bir problemle karşılaşıyoruz.
// ! Lazy Loading, kullanım açısından oldukça maliyetli ve performans düşürücü bir etkiye sahip yöntemdir.
// ! Dolayısıyla mümkün mertebe dikkatli olmalı ve özellikle navigation property'lerin döngüsel tetiklenme durumlarında lazy loading'i tercih etmemeye odaklanmalıyız.
// ! Aksi taktirde her bir tetiklemeye karşılık aynı sorguları üretip execute edecektir. Bu durumu N+1 Problemi olarak nitelendirmekteyiz.
//var region = await context.Regions.FindAsync(1);
//foreach (var employee in region.Employees)
//{
//    var orders = employee.Orders;
//    foreach (var order in orders)
//    {
//        Console.WriteLine(order.OrderDate);
//    }
//}
#endregion
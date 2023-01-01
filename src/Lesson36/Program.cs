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

#region Explicit Loading
/**

    * ? Oluşturulan sorguya eklenecek verilerin şartlara bağlı bir şekilde ihtiyaçlara istinaden yüklenmesini sağlayan bir yaklaşımdır.

        var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == 2);
        if (employee.Name == "Gençay")
        {
            var orders = await context.Orders.Where(o => o.EmployeeId == employee.Id).ToListAsync();
        }

    * * You can explicitly load a navigation property via the DbContext.Entry(...) API.

            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Load();

                context.Entry(blog)
                    .Reference(b => b.Owner)
                    .Load();
            }
*/
#endregion

#region Reference() & Collection() & Load()
/*

    * * The Load() method executes the SQL query in the database to get the data and fill up the specified reference or collection property in the memory.
    * * In the example below, context.Entry(student).Reference(s => s.StudentAddress).Load() loads the StudentAddress entity.
    * * The Reference() method is used to get an object of the specified reference navigation property and the Load() method loads it explicitly.
    ? ? Explicit Loading sürecinde ilişkisel olarak sorguya eklenmek istenen tablonun navigation property'si tekil bir türse bu tabloyu Reference() ile sorguya ekleyebilemekteyiz.

        using (var context = new SchoolContext())
        {
            var student = context.Students
                                .Where(s => s.FirstName == "Bill")
                                .FirstOrDefault<Student>();

            context.Entry(student).Reference(s => s.StudentAddress).Load();     // → loads StudentAddress
        }

    * * In the same way, context.Entry(student).Collection(s => s.StudentCourses).Load() loads the collection navigation property Courses of the Student entity.
    * * The Collection() method gets an object that represents the collection navigation property.
    ? ? Explicit Loading sürecinde ilişkisel olarak sorguya eklenmek istenen tablonun navigation property'si çoğul/koleksiyonel bir türse bu tabloyu Collection() ile sorguya ekleyebilemekteyiz.

        using (var context = new SchoolContext())
        {
            var student = context.Students
                                .Where(s => s.FirstName == "Bill")
                                .FirstOrDefault<Student>();

            context.Entry(student).Collection(s => s.StudentCourses).Load();    // → loads Courses collection
        }
*/
#endregion

#region Querying and Filtering related entities - Query()
/**

    * * You can also write LINQ-to-Entities queries to filter the related data before loading. 
    * * The Query() method enables us to write further LINQ queries for the related entities to filter out related data.
    * * This allows you to apply other operators over the query.
    * * For example, applying an aggregate operator over the related entities without loading them into memory:

        using (var context = new BloggingContext())
        {
            var blog = context.Blogs
                .Single(b => b.BlogId == 1);

            var postCount = context.Entry(blog)
                .Collection(b => b.Posts)
                .Query()
                .Count();
        }
*/
/*

    * * You can also filter which related entities are loaded into memory.
    * *

        using (var context = new BloggingContext())
        {
            var blog = context.Blogs
                .Single(b => b.BlogId == 1);

            var goodPosts = context.Entry(blog)
                .Collection(b => b.Posts)
                .Query()
                .Where(p => p.Rating > 3)
                .ToList();
        }
*/
#endregion

Console.WriteLine("Hello, World!");

#region SQL Queries
/**

    * * EF Core allows you to drop down to SQL queries when working with a relational database.
    * * SQL queries are useful if the query you want can't be expressed using LINQ, or if a LINQ query causes EF to generate inefficient SQL.
    * * SQL queries can return regular entity types or keyless entity types that are part of your model.

    * * $"..."
    * * While this syntax may look like regular C# string interpolation, the supplied value is wrapped in a DbParameter and the generated parameter name inserted where the {0} placeholder was specified.
    * * This makes > The FromSql safe from SQL injection attacks, and sends the value efficiently and correctly to the database.

    * ! Manuel bir şekilde tarafımızca oluşturulmuş olan sorguları EF Core tarafından execute edebilmek için o sorgunun
    * ! ...sonucunu karşılayacak bir entity'nin tasarlanmış ve bunun DbSet<T> olarak context nesnesine tanımlanmış olması gerekiyor.
*/
#endregion

#region Basic SQL queries
#region FromSql() - EF Core 7.0.
/**

    * * You can use FromSql() to begin a LINQ query based on a SQL query:
    * ! FromSql() can only be used directly on a DbSet. It cannot be composed over an arbitrary LINQ query.
    * ! FromSql() was introduced in EF Core 7.0. When using older versions, use FromSqlInterpolated() instead.


    var blogs = await _context.Blogs
                            .FromSql($"SELECT * FROM dbo.Blogs")
                            .ToListAsync();

*/
#region Stored Procedure Execute
/**

    * * SQL queries can be used to execute a stored procedure which returns entity data:

    var blogs = await _context.Blogs
                            .FromSql($"EXECUTE dbo.GetMostPopularBlogs")
                            .ToListAsync();
    var persons = await _context.Persons
                            .FromSql($"EXECUTE dbo.sp_GetAllPersons 4")
                            .ToListAsync();
*/
#endregion
#endregion
#region FromSqlInterpolated()
/**

    * * EF Core 7.0 sürümünden önce ham sorguları execute edebildiğimiz fonksiyondur.

    var persons = await _context.Persons
                            .FromSqlInterpolated($"SELECT * FROM Persons")
                            .ToListAsync();
*/
#endregion
#endregion

#region Passing parameters
/**
 
    * * When executing stored procedures, it can be useful to use named parameters in the SQL query string, especially when the stored procedure has optional parameters:
    * * If you need more control over the database parameter being sent, you can also construct a DbParameter and supply it as a parameter value.
    * * This allows you to set the precise database type of the parameter, or facets such as its size, precision or length:

    * ! example - 1
    int personId = 3;

    var persons = await _context.Persons
                        .FromSql($"SELECT * FROM Persons Where PersonId = {personId}")
                        .ToListAsync();

    * ! example - 2
    int personId = 3;

    var persons = await _context.Persons
                        .FromSql($"EXECUTE dbo.sp_GetAllPersons {personId}")
                        .ToListAsync();

    * ! example - 3
    SqlParameter personId = new("PersonId", "3");
    personId.DbType = System.Data.DbType.Int32;
    personId.Direction = System.Data.ParameterDirection.Input;

    var persons = await _context.Persons
                        .FromSql($"SELECT * FROM Persons Where PersonId = {personId}")
                        .ToListAsync();

    * ! example - 4
    SqlParameter personId = new("aaa", "3");

    var persons = await _context.Persons
                        .FromSql($"EXECUTE dbo.sp_GetAllPersons @PersonId = {personId}")
                        .ToListAsync();
    * ! example - 5
    var user = new SqlParameter("user", "johndoe");

    var blogs = await _context.Blogs
                        .FromSql($"EXECUTE dbo.GetMostPopularBlogsForUser @filterByUser={user}")
                        .ToList();
    var blogs = await _context.Blogs
                        .FromSql($"EXECUTE dbo.GetMostPopularBlogsForUser {user}")
                        .ToList();

*/
#endregion

#region Dynamic SQL and parameters
/**

    * * If you've decided you do want to dynamically construct your SQL, you'll have to use FromSqlRaw, which allows interpolating variable data directly into the SQL string, instead of using a database parameter:i
    * * Accepting a column name from a user may allow them to choose a column that isn't indexed, making the query run extremely slowly and overload your database;
    * * ...or it may allow them to choose a column containing data you don't want exposed.
    * * This code doesn't work, since databases do not allow parameterizing column names (or any other part of the schema).
    * ! EF Core dinamik olarak oluşturulan sorgularda özellikle kolon isimleri parametreleştirilmişse o sorguyu ÇALIŞTIRMAYACAKTIR!

    * ! example - 1 [doesn't work]
    var propertyName = "User";
    var propertyValue = "johndoe";

    var blogs = await _context.Blogs
                        .FromSql($"SELECT * FROM [Blogs] WHERE {propertyName} = {propertyValue}")
                        .ToListAsync();

    * ! example - 2 [doesn't work]
    string columnName = "PersonId", value = "3";
    var persons = await _context.Persons
                        .FromSql($"Select * From Persons Where {columnName} = {value}")
                        .ToListAsync();


    * * FromSqlRaw()
    * * If you've decided you do want to dynamically construct your SQL, you'll have to use FromSqlRaw(), which allows interpolating variable data directly into the SQL string, instead of using a database parameter:
    * ! FromSql() ve FromSqlInterpolated() yöntemlerinde SQL Injection gibi güvenlik önlemleri alınmış vaziyettedir.
    * ! Fakat sorguları dinamik olarak oluşturuyorsanız burada güvenlikten geliştirici sorumludur.

    
    * ! example - 1
    var columnName = "Url";
    var columnValue = new SqlParameter("columnValue", "http://SomeURL");

    var blogs = await _context.Blogs
                        .FromSqlRaw($"SELECT * FROM [Blogs] WHERE {columnName} = @columnValue", columnValue)
                        .ToListAsync();
    
    * ! example - 2
    string columnName = "PersonId";
    SqlParameter value = new("PersonId", "3");

    var persons = await _context.Persons
                        .FromSqlRaw($"Select * From Persons Where {columnName} = @PersonId", value)
                        .ToListAsync();
*/
#endregion

#region SqlQuery - Entiy Olmayan Scalar Sorguların Çalıştırılması - Non Entity - EF Core 7.0
/**

    * * Entity'si olmayan scalar sorguların çalıştırılıp sonucunu elde etmemizi sağlayan yeni bir fonksiyondur.

    var data = await _context.Database
                        .SqlQuery<int>($"SELECT PersonId FROM Persons")
                        .ToListAsync();

    * * SqlQuery'de LINQ operatörleriyle sorguya ekstradan bir müdahalede/katkıda bulunacaksanız bu sorgu neticesinde gelecek olan kolonun adını 'Value' olarak bildirmeniz gerekmektedir.
    * * Haliyle bu durumdan dolayı LINQ ile verilen şart ifadeleri statik olarak 'Value' kolonuna göre tasarlanmıştır.
    * ! O yüzden bu şekilde bir çalışma zorunlu gerekmektedir.

    var data = await _context.Database
                        .SqlQuery<int>($"SELECT PersonId Value FROM Persons")
                        .Where(x => x > 5)
                        .ToListAsync();
    

*/
#endregion

#region ExecuteSql()
/**
    * * Insert, update, delete

    await _context.Database
                .ExecuteSqlAsync($"Update Persons SET Name = 'Fatma' WHERE PersonId = 1");

*/
#endregion

#region Limitations
/**

    * * There are a few limitations to be aware of when returning entity types from SQL queries:
   
        1. The SQL query must return data for all properties of the entity type.
        2. The column names in the result set must match the column names that properties are mapped to.
            * ! Note that this behavior is different from EF6; EF6 ignored property-to-column mapping for SQL queries, and result set column names had to match those property names. 
        3. The SQL query can't contain related data. However, in many cases you can compose on top of the query using the Include() operator to return related data.
            * ! SQL Sorgusu Join yapısı İÇEREMEZ!
            var persons = await _context.Persons
                                .FromSql($"SELECT * FROM Persons")
                                .Include(p => p.Orders)
                                .ToListAsync();
*/
#endregion

#region sp_GetAllPersons
//CREATE PROC sp_GetAllPersons
//(
//	@PersonId INT NULL
//)AS
//BEGIN
//	IF @PersonId IS NULL
//		SELECT * FROM Persons
//	ELSE
//		SELECT * FROM Persons WHERE PersonId = @PersonId
//END   
#endregion

Console.WriteLine("Hello, World!");

#region Working with Scalar and Inline Functions in Entity Framework Core
/**

    * * A function in SQL Server is a subprogram that is used to perform an action such as complex calculation and returns the result of the action as a value.
    * * There are two types of functions in SQL Server, such as

        1. System Defined Function
        2. User-Defined Function
    * ! The functions which are already defined by the system and ready to be used by the developer are called system-defined functions
    ... whereas if the function is defined by the user or developer then such types of functions are called the user-defined function.

    * ? Some functions take parameters; do some processing and returning some results back. For example SELECT SQUARE(3)
    * ? Some functions may not take any parameters, but returns some result, for example, SELECT GETDATE()

    * * Fonksiyonlar tamamen işimizi kolaylaştırmak adına sürekli olarak tekrarladığımız sql sorgularına tek bir noktadan erişmemizi sağlar. 
    * * Bu da bize hızlı bir erişim imkanı, hızlı bir hata kontrol mekanizması, çabuk müdahale, sorgu tekrarlamama gibi imkanları verir.
    * * Fonksiyonlar, View ve Store Procedure kullanmanın avantajlarını kendi bünyelerinde toplamışlardır.
    * * Sadece 'girdi' parametreleri alırlar. Geriye mutlaka bir şey dönmek zorundalar.

    * * In SQL Server, we can create three types of User-Defined Functions, such as

        1. Scalar-Valued Function
        2. Inline Table-Valued Functions
        3. Multi-Statement Table-Valued Functions
        4. Aggragate Function
*/
#region Scalar Functions in Entity Framework Core
/**

    * ! The user-defined function which returns only a single value (scalar value) is known as the Scalar Valued Function. 
    * * Scalar Functions: Bir veya daha fazla parametre alabilen ve geriye sayısal türde tek bir değer döndüren fonksiyonlardır.
    * * Bir Scalar fonksiyon oluşturmak için CREATE FUNCTION statement'ı kullanılır.
    
        1. First, specify the name of the function after the CREATE FUNCTION keywords.
        2. The schema name is optional. If you don’t explicitly specify it, SQL Server uses dbo by default.
        3. Second, specify a list of parameters surrounded by parentheses after the function name.
        4. Third, specify the data type of the return value in the RETURNS statement.
        5. Finally, include a RETURN statement to return a value inside the body of the function.

        CREATE FUNCTION [schema_name.]function_name (parameter_list)
        RETURNS data_type AS
        BEGIN
            statements
            RETURN value
        END

        CREATE FUNCTION sales.udfNetSale(
        @quantity INT,
        @list_price DEC(10,2),
        @discount DEC(4,2)
        )
        RETURNS DEC(10,2)
        AS 
        BEGIN
            RETURN @quantity * @list_price * (1 - @discount);
        END;

        Create Function Fn_ToplamaYap(@sayi1 int,@sayi2 int)
        Returns int
        As
        Begin
        Declare @toplam int
        Set @toplam = @sayi1+ @sayi2
        return @toplam
        End

    * * Boş bir migration oluşturulmalıdır.
    * * Migration içerisindeki Up fonksiyonunda Scalar fonksiyonun create komutları, Down fonksiyonunda ise drop komutları yazılmalıdır.

        migrationBuilder.Sql($@"
                CREATE FUNCTION getPersonTotalOrderPrice(@personId INT)
                    RETURNS INT
                AS
                BEGIN
                    DECLARE @totalPrice INT
                    SELECT @totalPrice = SUM(o.Price) FROM Persons p
                    JOIN Orders o
                        ON p.PersonId = o.PersonId
                    WHERE p.PersonId = @personId
                    RETURN @totalPrice
                END
                ");

        migrationBuilder.Sql($@"DROP FUNCTION getPersonTotalOrderPrice");

    * * Migration migrate edilmelidir.
    * * Eldeki Scalar fonksiyonu temsil edecek context nesnesi içinde bir imza oluşturulur.

        public int GetPersonTotalOrderPrice(int personId)
                    => throw new Exception();

    * * Daha sonra OnModelCreating içinde yukarıdaki imza ile bir bind işlemi gerçekleştirilir.
    * ! HasDbFunction(): Veritabanı seviyesindeki herhangi bir fonksiyonu EF Core/yazılım kısmında bir metoda bind etmemizi sağlayan fonksiyondur.

        modelBuilder
            .HasDbFunction(typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplicationDbContext.GetPersonTotalOrderPrice), new[] { typeof(int) }))
            .HasName("getPersonTotalOrderPrice");

    * * Kullanımı:

        var persons = await (from person in context.Persons
                    where context.GetPersonTotalOrderPrice(person.PersonId) > 500
                    select person).ToListAsync();
*/
#endregion


#region Inline Functions in Entity Framework Core
/**

    * * Inline Functions: Geriye bir değer değil, tablo döndüren fonksiyonlardır.
    
    * * Boş bir migration oluşturulmalıdır.
    * * Migration içerisindeki Up fonksiyonunda Inline fonksiyonun create komutları, Down fonksiyonunda ise drop komutları yazılmalıdır.

        migrationBuilder.Sql($@"
                CREATE FUNCTION bestSellingStaff(@totalOrderPrice INT = 10000)
	                RETURNS TABLE
                AS
                RETURN 
                SELECT TOP 1 p.Name, COUNT(*) OrderCount, SUM(o.Price) TotalOrderPrice FROM Persons p
                JOIN Orders o
	                ON p.PersonId = o.PersonId
                GROUP By p.Name
                HAVING SUM(o.Price) < @totalOrderPrice
                ORDER By OrderCount DESC");
        
        migrationBuilder.Sql($@"DROP FUNCTION bestSellingStaff");

    * * Migration migrate edilmelidir.
    * * Eldeki Inline fonksiyonu karşılayabilecek bir class/model oluşturulur.
        public class BestSellingStaff
        {
            public string Name { get; set; }
            public int OrderCount { get; set; }
            public int TotalOrderPrice { get; set; }
        }
    * * Eldeki Inline fonksiyonu temsil edecek context nesnesi içinde bir imza oluşturulur.

        public IQueryable<BestSellingStaff> BestSellingStaff(int totalOrderPrice = 10000)
                => FromExpression(() => BestSellingStaff(totalOrderPrice));

    * * Daha sonra OnModelCreating içinde yukarıdaki imza ile bir bind işlemi gerçekleştirilir.
    * ! HasDbFunction(): Veritabanı seviyesindeki herhangi bir fonksiyonu EF Core/yazılım kısmında bir metoda bind etmemizi sağlayan fonksiyondur.

        modelBuilder
            .HasDbFunction(typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplicationDbContext.BestSellingStaff), new[] { typeof(int) }))
            .HasName("bestSellingStaff");

        modelBuilder
            .Entity<BestSellingStaff>()
            .HasNoKey();

    * * Kullanımı:

        var persons = await context.BestSellingStaff(3000).ToListAsync();

        foreach (var person in persons)
        {
            Console.WriteLine($"Name : {person.Name} | Order Count : {person.OrderCount} | Total Order Price : {person.TotalOrderPrice}");
        }
*/
#endregion


#region Server'dan hazir Stored Function Kullanmak
/**

    * * Fonksiyon veritabanında yazılır ve kaydedilir.

    * ? Bir parametre almayan ve bir tablo dönen...
        * Bu fonksiyunu karşılayacak bir class model'ı veya değişkeni oluşturulur.
        * Bu model context'e DbSet<> olarak eklenir.
            - modelBuilder.Entity<ProductFull>().ToFunction("fn_product_full");
            - var products = await _context.ProductFulls.ToListAsync();
    
    * ? Bir parametre alan ve bir tablo dönen...
        * Eldeki fonksiyonu temsil edecek context nesnesi içinde bir imza oluşturulur.
            - public IQueryable<ProductWithFeature> GetProductWithFeatures(int categoryId) => FromExpression(() => GetProductWithFeatures(categoryId));
        * Daha sonra OnModelCreating içinde yukarıdaki imza ile bir bind işlemi gerçekleştirilir.
            - modelBuilder.HasDbFunction(typeof(AppDbContext).GetMethod(nameof(GetProductWithFeatures), new[] { typeof(int) })!).HasName("fn_product_full_with_parameters");
        - var product = await _context.GetProductWithFeatures(1).Where(x => x.Width > 100).ToListAsync();

    * ? Scaler
        * Eldeki fonksiyonu temsil edecek context nesnesi içinde bir imza oluşturulur.
            - public int GetProductCount(int categoryId) => throw new NotSupportedException("Bu method ef core tarafından çalıştırılmaktadır.");
        * Daha sonra OnModelCreating içinde yukarıdaki imza ile bir bind işlemi gerçekleştirilir.
            - modelBuilder.HasDbFunction(typeof(AppDbContext).GetMethod(nameof(GetProductCount), new[] { typeof(int) })!).HasName("fc_get_product_count");
        - var categories = await _context.Categories.Select(x => new
            {
                CategoryName = x.Name,
                ProductCount = _context.GetProductCount(x.Id)
            }).Where(x => x.ProductCount > 10).ToListAsync();
    * ! HasNoKey() → EF Core tarafından track edilmez.
*/
#endregion

#endregion
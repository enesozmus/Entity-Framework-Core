Console.WriteLine("Hello, World!");

#region Stored Procedures
/**

    * * Veritabanında CRUD gibi işlemlerde, her seferinde kodu tekrar yazmamız ve derlememiz gerekmektedir.
    * * Durum böyle olunca hem zaman hem de derleme açısından perormans kaybı olmaktadır.
    * * Bu gibi durumlarda Saklı Yordam (Store Procedure), programlamada kullanılan ifadelere göre kod yazmamızı sağlar.
    * * Böylece her seferinde aynı işlemleri yapma gereksinimi duymadan zamandan tasarruf etmiş oluruz.
    * ! Dolayısıyla 'tekrar kullanılabilirliği sağlamak' istediğimizde kullanılır.
    * ! Daha hızlı yürütme (execution) istediğimizde kullanılır. 
    * ! Ağ trafiğini azaltmak istediğimizde kullanılır.

    * ? Stored Procedure, View'ler gibi kompleks sorgularımızı tekrar kullanılabilir bir hale getirmemizi sağlayan veritabanı nesnesidir. 
    * ? View'ler tablo vari bir davranış sergilerken, Stored Procedure'le ise fonksiyonel bir davranış sergilerler.

    * ! Bir defa derledikten sonra, tekrar derlemeye gerek kalmaz. Veritabanında derlenmiş bir execution plan halinde saklanır.
*/
#endregion


#region Kod Tarafinda Stored Procedure Oluşturma
/**

    * * Boş bir migration oluşturulmalıdır.
    * * Migration içerisindeki Up fonksiyonunda Stored Procedure'ün create komutları, Down fonksiyonunda ise drop komutları yazılmalıdır.

        - migrationBuilder.Sql($@"CREATE PROCEDURE...Buraya SQL kodlarıyla serverda çalışacak Stored Procedure yazılır.");
        - migrationBuilder.Sql($@"DROP PROCEDURE... Stored Procedure adı");

        * ! Example
                    migrationBuilder.Sql($@"
                        CREATE PROCEDURE sp_PersonOrders
                        AS
	                        SELECT p.Name, COUNT(*) [Count] FROM Persons p
	                        JOIN Orders o
		                        ON p.PersonId = o.PersonId
	                        GROUP By p.Name
	                        ORDER By COUNT(*) DESC
                        ");

    * * Migration migrate edilmelidir.
    * ! Stored Procedure'ü EF Core üzerinden sorgulayabilmek/karşılayabilmek için Stored Procedure kolonlarına/property'lerine uygun
    * ! ...bir entity oluşturulması ve context'e DbSet<> olarak eklenmesi gerekmektedir.
    * ! İlgili class [NotMapped] ile işaretlenir.

            [NotMapped]
        - public class ModelforSP {...}

    * * DbSet<>'in Bir Stored Procedure Olduğunu Bildirmek

        -  public DbSet<ModelforSP> ModelforSPs { get; set; }
        -  modelBuilder.Entity<ModelforSP>().HasNoKey();

    * * DbSet<TEntity>.FromSql() - Stored Procedure'ü kullanmak

        - var datas = await _context.ModelforSPs
                            .FromSql($"EXEC sp_name")
                            .ToListAsync();

    * ! Stored Procedure'lerde Primary Key olmamasına dikkat ederiz! Bu yüzden ilgili DbSet<>'in HasNoKey() ile işaretlenmesi gerekemktedir.
    * ! HasNoKey() → EF Core tarafından track edilmez.
*/
#endregion


#region Geriye Değer Döndüren Stored Procedure Kullanma
/**
     * * Create
            migrationBuilder.Sql($@"
                    CREATE PROCEDURE sp_bestSellingStaff
                    AS
	                    DECLARE @name NVARCHAR(MAX), @count INT
	                    SELECT TOP 1 @name = p.Name, @count = COUNT(*) FROM Persons p
	                    JOIN Orders o
		                    ON p.PersonId = o.PersonId
	                    GROUP By p.Name
	                    ORDER By COUNT(*) DESC
	                    RETURN @count
                    ");

    ** Drop
            migrationBuilder.Sql($@"DROP PROCEDURE sp_bestSellingStaff");
*/

// SqlParameter countParameter = new()
// {
//    ParameterName = "count",
//    SqlDbType = System.Data.SqlDbType.Int,
//    Direction = System.Data.ParameterDirection.Output
// };
// await context.Database.ExecuteSqlRawAsync($"EXEC @count = sp_bestSellingStaff", countParameter);
// Console.WriteLine(countParameter.Value);
#endregion


#region OUTPUT
/**

    * * Create
            migrationBuilder.Sql($@"
                CREATE PROCEDURE sp_PersonOrders2
                (
                    @PersonId INT,
                    @Name NVARCHAR(MAX) OUTPUT
                )
                AS
                SELECT @Name = p.Name FROM Persons p
                JOIN Orders o
                    ON p.PersonId = o.PersonId
                WHERE p.PersonId = @PersonId
                ");
    * * Drop
            migrationBuilder.Sql($"DROP PROC sp_PersonOrders2");
*/
// SqlParameter nameParameter = new()
// {
//    ParameterName = "name",
//    SqlDbType = System.Data.SqlDbType.NVarChar,
//    Direction = System.Data.ParameterDirection.Output,
//    Size = 1000
// };
// await context.Database.ExecuteSqlRawAsync($"EXECUTE sp_PersonOrders2 7, @name OUTPUT", nameParameter);
// Console.WriteLine(nameParameter.Value);
#endregion


#region Server'dan hazir Stored Procedure Kullanmak
/**

    * * Stored Procedure veritabanında yazılır ve kaydedilir.
    * * Bu Stored Procedure', karşılayacak bir class model'ı oluşturulur. -public class Abcd {...}
    * * Bu model context'e DbSet<> olarak eklenir.

    * ! HasNoKey() → EF Core tarafından track edilmez.
*/
#region Example - 1
// List<Product>? products = await _context.Products
//                                   .FromSqlRaw("EXEC sp_getAllProducts")
//                                   .ToListAsync();

/*
  ** Veritabanında Karşılığı Olan Bir Tablo **

    CREATE PROCEDURE sp_getAllProducts
    as
    BEGIN
    SELECT * FROM Products
    END
*/
#endregion

#region Example - 2
// var products2 = await _context.AModelForSps
//                           .FromSqlRaw("EXEC sp_getAllProducts2")
//                           .ToListAsync();

/*
  ** Veritabanında Karşılığı Olmayan Bir Tablo **
  CREATE PROCEDURE sp_getAllProducts2
  as
  BEGIN
  SELECT product.Id, product.Name, product.Price, category.Name 'CategoryName', productFeature.Width, productFeature.Height FROM Products product
  JOIN Categories category ON product.CategoryId = category.Id
  JOIN ProductFeatures productFeature ON product.Id = productFeature.Id
  END
*/
// [NotMapped]
// public class AModelForSp
// {
//   public int Id { get; set; }
//   public string Name { get; set; }
//   public decimal Price { get; set; }

//   public string CategoryName { get; set; }
//   public int? Width { get; set; }
//   public int? Height { get; set; }
// }
// public DbSet<AModelForSp> AModelForSps { get; set; }
// modelBuilder.Entity<AModelForSp>().HasNoKey(); 
#endregion

#region Example - 3
// var categoryId_parameter = 2;
// var price_parameter = 100;

// List<AModelForSp>? products3 = await _context.AModelForSps
//                                   .FromSqlRaw($"EXECUTE sp_getAllProducts3_with_paramaters {categoryId_parameter}, {price_parameter}")
//                                   .ToListAsync();

/*
  ** Dışarıdan parametre alan bir Store Procedure **

    CREATE PROCEDURE sp_getAllProducts3_with_paramaters
    @categoryId int,
    @price decimal(9,2)
    as
    BEGIN
    SELECT product.Id, product.Name, product.Price, category.Name 'CategoryName', productFeature.Width, productFeature.Height FROM Products product
    JOIN Categories category ON product.CategoryId = category.Id
    JOIN ProductFeatures productFeature ON product.Id = productFeature.Id
    WHERE product.CategoryId = @categoryId and product.Price > @price
    END

    EXECUTE sp_getAllProducts3_with_paramaters 2, 100
*/
#endregion
#endregion

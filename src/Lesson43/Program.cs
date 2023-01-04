Console.WriteLine("Hello, World!");

#region Context Nesnesi İçerisindeki Database Property'si - _context.Database.
/**

  * * DbContext.Database Property (Microsoft.EntityFrameworkCore)
  * * Provides access to database related information and operations for this context.
*/
#endregion


#region Transactions
/**

  * * Transactions allow several database operations to be processed in an atomic manner.
  * * If the transaction is committed, all of the operations are successfully applied to the database.
  * * If the transaction is rolled back, none of the operations are applied to the database.

  * ? Default transaction behavior
  * * By default, if the database provider supports transactions, all changes in a single call to SaveChanges() are applied in a transaction.
  * * If any of the changes fail, then the transaction is rolled back and none of the changes are applied to the database.
  * * This means that SaveChanges() is guaranteed to either completely succeed, or leave the database unmodified if an error occurs.
  * * For most applications, this default behavior is sufficient.
  * ! You should only manually control transactions if your application requirements deem it necessary.

  * ? DbContext.Database
  * * You can use the DbContext.Database API to begin, commit, and rollback transactions.
*/
#endregion


#region _context.Database.BeginTransaction()
/**

  * ? IDbContextTransaction transaction = _context.Database.BeginTransaction();
  * ! Starts a new transaction.
  * * EF Core, transaction yönetimini otomatik bir şekilde kendisi gerçekleştirmektedir.
  * * Transaction yönetimini manuel olarak anlık ele almak istiyorsak BeginTransaction() fonksiyonunu kullanabiliriz.
*/
#endregion


#region _context.Database.CommitTransaction()
/**

  * ! Applies the outstanding operations in the current transaction to the database.
  * * EF Core üzerinde yapılan çalışmaların commit edilebilmesi için kullanılan bir fonksiyondur.
*/
#endregion


#region _context.Database.RollbackTransaction()
/**

  * ! Discards the outstanding operations in the current transaction.
  * * EF Core üzerinde yapılan çalışmaların rollback edilebilmesi için kullanılan bir fonksiyondur.
*/
#endregion


#region _context.Database.CanConnect()
/**

  * ? bool connect = _context.Database.CanConnect();
  * ! Determines whether or not the database is available and can be connected to.
  * ! Any exceptions thrown when attempting to connect are caught and not propagated to the application.
  * ! The configured connection string is used to create the connection in the normal way, so all configured options such as timeouts are honored.
  * ? returns true if the database is available; false otherwise.
  * * Verilen connection string'e karşılık bağlantı kurulabilir bir veritabanı var mı yok mu bunun bilgisini bool türde veren bir fonksiyondur.
*/
#endregion


#region _context.Database.EnsureCreated()
/**

  * ! Ensures that the database for the context exists.
  * ! If the database exists and has any tables, then no action is taken. 
  * ! If the database exists but does not have any tables, then the Entity Framework model is used to create the database schema.
  * ! If the database does not exist, then the database is created and the Entity Framework model is used to create the database schema.
  * ! Note that this API does **not** use migrations to create the database. In addition, the database that is created cannot be later updated using migrations.
  * ! Returns true if the database is created, false if it already existed.
  * * EF Core'da tasarlanan veritabanını migration kullanmaksızın, runtime'da yani kod üzerinde veritabanı sunucusuna inşa edebilmek için kullanılan bir fonksiyondur.
*/
#endregion


#region _context.Database.EnsureDeleted()
/**

  * ! Ensures that the database for the context does not exist. If it does not exist, no action is taken.
  * ! If it does exist then the database is deleted.
  * ! Returns true if the database is deleted, false if it did not exist.
  * * İnşa edilmiş veritabanını runtime'da silebilmemizi sağlayan bir fonksiyondur.
*/
#endregion


#region _context.Database.GenerateCreateScript()
/**

  * ! Generates a script to create all tables for the current model.
  * ! Returns a SQL script.
  * ! string? script = _context.Database.GenerateCreateScript();
  * * Context nesnesinde yapılmış olan veritabanı tasarımı her ne ise ona uygun bir SQL Script'ini string olarak veren fonksiyondur.
*/
#endregion


#region _context.Database.ExecuteSql($"") - EF Core 7
/**

  * ! Executes the given SQL against the database and returns the number of rows affected.
  * * Veritabanına yapılacak Insert, Update ve Delete sorgularını yazabildiğimiz bir fonksiyondur.
  * * Bu fonksiyondur işlevsel olarak alacağı parametreleri SQL Injection saldırılarına karşı korumaktadır.

    - string name = Console.ReadLine();
    - var result = context.Database.ExecuteSql($"INSERT Persons VALUES('{name}')");
*/
#endregion


#region _context.Database.ExecuteSqlRaw()
/**

  * ! Executes the given SQL against the database and returns the number of rows affected.
  * * Veritabanına yapılacak Insert, Update ve Delete sorgularını yazabildiğimiz bir fonksiyondur.
  * * Bu metotta ise sorguyu SQL Injection saldırılarına karşı koruma görevi geliştirinin sorumluluğundadır.
*/
#endregion


#region _context.Database.GetMigrations()
/**

  * ! Gets all the migrations that are defined in the configured migrations assembly.
  * ! Returns the list of migrations.
  * ! IEnumerable<string>? migs = _context.Database.GetMigrations();
  * * Uygulamada üretilmiş olan tüm migration'ları runtime'da programatik olarak elde etmemizi sağlayan fonksiyondur.
*/
#endregion


#region _context.Database.GetAppliedMigrations()
/**

  * ! Gets all migrations that have been applied to the target database.
  * ! Returns the list of migrations.
  * ! IEnumerable<string>? migs = _context.Database.GetAppliedMigrations();
  * * Uygulamada migrate edilmiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
*/
#endregion


#region _context.Database.GetPendingMigrations()
/**

  * ! Gets all migrations that are defined in the assembly but haven't been applied to the target database.
  * ! Returns the list of migrations.
  * ! IEnumerable<string>? migs = _context.Database.GetPendingMigrations();
  * * Uygulamada migrate edilmemiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
*/
#endregion


#region _context.Database.Migrate()
/**

  * ! Applies any pending migrations for the context to the database.
  * ! Will create the database if it does not already exist.
  * * Migration'ları programatik olarak runtime'da migrate etmek için kullanılan bir fonksiyondur.
*/
#endregion


#region _context.Database.OpenConnection() & _context.Database.CloseConnection()
/**

  * ! Opens the underlying System.Data.Common.DbConnection.
  * ! Closes the underlying System.Data.Common.DbConnection.
  * * Veritabanı bağlantısını manuel olarak açar.
  * * Veritabanı bağlantısını manuel olarak kapatır.
*/
#endregion


#region _context.Database.GetConnectionString()
/**

  * ! Gets the underlying connection string configured for this Microsoft.EntityFrameworkCore.DbContext.
  * ! Returns the connection string.
  * ! string? connectionstring =_context.Database.GetConnectionString();
  * ! Console.WriteLine(_context.Database.GetConnectionString());
  * * İlgili context nesnesinin o an için kullandığı ConnectionString değerini elde etmemizi sağlayan fonksiyondur.
*/
#endregion


#region _context.Database.GetDbConnection()
/**

  * ! SqlConnection connection = (SqlConnection)context.Database.GetDbConnection();
  * ! Returns the System.Data.Common.DbConnection
  * * EF Core'un kullanmış olduğu Ado.NET altyapısının kullandığı DbConnection nesnesini elde etmemizi sağlayan bir fonksiyondur.
*/
#endregion


#region _context.Database.ProviderName. - Property
/**

  * ! Console.WriteLine(_context.Database.ProviderName);
  * * EF Core'un kullanmış olduğu provider neyse onun bilgisini getiren bir proeprty'dir.
*/
#endregion
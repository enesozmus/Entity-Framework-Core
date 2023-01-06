Console.WriteLine("Hello, World!");

#region Using Transactions
/*

  * Transactions allow several database operations to be processed in an atomic manner.
  * If the transaction is committed, all of the operations are successfully applied to the database.
  * If the transaction is rolled back, none of the operations are applied to the database.
  + Transaction'ın genel amacı veritabanındaki tutarlılık durumunu korumaktadır.
*/
#endregion


#region Default transaction behavior
/*

  * By default, if the database provider supports transactions, all changes in a single call to SaveChanges are applied in a transaction.
  * If any of the changes fail, then the transaction is rolled back and none of the changes are applied to the database.
  * This means that SaveChanges is guaranteed to either completely succeed, or leave the database unmodified if an error occurs.
  * For most applications, this default behavior is sufficient.
  ! You should only manually control transactions if your application requirements deem it necessary.
*/
#endregion


#region Controlling transactions
/*

  * You can use the DbContext.Database API to begin, commit, and rollback transactions.
  * EF Core'da, transaction'lar üzerinde kontrolü ele almak istiyorsak DbContext.Database.BeginTransaction() çağrılır.
  ? DbContext.Database.BeginTransaction(): Starts a new transaction.
  ! IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

  using var _context = new BloggingContext();
  using var transaction = _context.Database.BeginTransaction();

  try
  {
      _context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
      _context.SaveChanges();

      _context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/visualstudio" });
      _context.SaveChanges();

      var blogs = _context.Blogs
                    .OrderBy(b => b.Url)
                    .ToList();

      ! Commit transaction if all commands succeed, transaction will auto-rollback
      ! when disposed if either commands fails
      transaction.Commit();
  }
  catch (Exception)
  {
    + TODO: Handle failure
  }
*/
#endregion


#region Savepoints
/*

  * This feature was introduced in EF Core 5.0.
  * When SaveChanges is invoked and a transaction is already in progress on the context, EF automatically creates a savepoint before saving any data.
  * Savepoints are points within a database transaction which may later be rolled back to, if an error occurs or for any other reason.
  * If SaveChanges encounters any error, it automatically rolls the transaction back to the savepoint, leaving the transaction in the same state as if it had never started.
  * This allows you to possibly correct issues and retry saving, in particular when optimistic concurrency issues occur.
  + Savepoints özelliği bir transaction içerisinde istenildiği kadar kullanılabilir.

  ! It's also possible to manually manage savepoints, just as it is with transactions.
  ! The following example creates a savepoint within a transaction, and rolls back to it on failure:

  using var _context = new BloggingContext();
  using var transaction = _context.Database.BeginTransaction();

  try
  {
      _context.Blogs.Add(new Blog { Url = "https://devblogs.microsoft.com/dotnet/" });
      _context.SaveChanges();

      transaction.CreateSavepoint("BeforeMoreBlogs");

      _context.Blogs.Add(new Blog { Url = "https://devblogs.microsoft.com/visualstudio/" });
      _context.Blogs.Add(new Blog { Url = "https://devblogs.microsoft.com/aspnet/" });
      _context.SaveChanges();

      transaction.Commit();
  }
  catch (Exception)
  {
      ! If a failure occurred, we rollback to the savepoint and can continue the transaction
      transaction.RollbackToSavepoint("BeforeMoreBlogs");

      + TODO: Handle failure, possibly retry inserting blogs
  }
*/
#region CreateSavepoint() and RollbackToSavepoint()
/*
  ? transaction.CreateSavepoint(""):

    - Creates a savepoint in the transaction.
    - This allows all commands that are executed after the savepoint was established to be rolled back, restoring the transaction state to what it was at the time of the savepoint.
    
  ? transaction.RollbackToSavepoint(""):

    - Rolls back all commands that were executed after the specified savepoint was established.
*/
#endregion
#endregion


#region public sealed class TransactionScope : IDisposable { ... }
/*

  * Makes a code block transactional.
  * This class cannot be inherited.
  ? TransactionScope transactionScope = new();
  ? transactionScope.Complete(); → Compote fonksiyonu yapılan veritabanı işlemlerinin commit edilmesini sağlar.
*/
#endregion
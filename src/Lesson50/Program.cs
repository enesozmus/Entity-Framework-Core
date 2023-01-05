Console.WriteLine("Hello, World!");

#region Connection Resiliency
/*

  + EF Core üzerinde yapılan veritabanı çalışmaları sürecinde ister istemez veritabanı bağlantısında kopuşlar/kesintiler meydana gelebilmektedir.
  + Connection Resiliency ile kopan bağlantıyı tekrar kurmak için gerekli 'tekrar bağlantı taleplerinde' bulunabilir.
  + Ayrıca execution strategy dediğimiz davranış modellerini belirleyerek bağlantıların kopması durumunda tekrar edecek olan sorguları baştan sona yeniden tetikleyebiliriz.
  * Connection resiliency automatically retries failed database commands.
  * The feature can be used with any database by supplying an "execution strategy", which encapsulates the logic necessary to detect failures and retry commands.
  ? As an example, the SQL Server provider includes an execution strategy that is specifically tailored to SQL Server.
  ! Enabling retry on failure causes EF to internally buffer the resultset, which may significantly increase memory requirements for queries returning large resultsets.
*/
#endregion


#region Execution Strategies
/*

  * EF Core ile yapılan bir işlem sürecinde veritabanı bağlantısı koptuğu taktirde yeniden bağlantı denenirken yapılan davranışa/alınan aksiyona Execution Strategy denmektedir.
  * Bu stratejiyi default değerlerde kullanabileceğimiz gibi custom olarak da kendimize göre özelleştirebilir ve bağlantı koptuğu durumlarda istediğimiz aksiyonları alabiliriz.
*/
#endregion


#region Default Execution Strategy
/*

  * Connection Resiliency için EnableRetryOnFailure() yöntemini kullanıyorsak bu default Execution Strategy'ye karşılık gelecektir.
  * Default değerlerin kullanılabilmesi için EnableRetryOnFailure() yönteminin parametresiz overload'ı kullanılabilir.
  + EnableRetryOnFailure() : Uygulama sürecinde veritabanı bağlantısı koptuğu taktirde bu yapılandırma sayesinde bağlantıyı tekrardan kurmaya çalışabiliyoruz.

    ? MaxRetryCount() : Yeniden bağlantı sağlanması durumunun kaç kere gerçekleştirileceğini bildirmektedir. Defualt değeri 6'dır.
    ? MaxRetryDelay() : Yeniden bağlantı sağlanması durumunun periyodunu bildirmektedir. Default değeri 30'dur.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder
            .UseSqlServer("<connection string>", builder => builder.EnableRetryOnFailure());
    }
*/
#endregion


#region EnableRetryOnFailure()
/*

  * An execution strategy is specified when configuring the options for your context.
  * This is typically in the OnConfiguring method of your derived context:

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
      + example
      optionsBuilder.UseSqlServer("<connection string>", builder => builder.EnableRetryOnFailure
      (
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(15),
        errorNumbersToAdd: new[] { 4060 })
      )
      .LogTo
      (
        filter: (eventId, level) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
        logger: eventData =>
        {
          Console.WriteLine($"Bağlantı tekrar kurulmaktadır.");
        }
      );
  }
*/
#endregion


#region Custom execution strategy
/*

  * There is a mechanism to register a custom execution strategy of your own if you wish to change any of the defaults.
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseMyProvider(
                "<connection string>",
                options => options.ExecutionStrategy(...));

        + or
        optionsBuilder
          .UseSqlServer("<connection string>", builder => builder.ExecutionStrategy
          (
            dependencies => new CustomExecutionStrategy(dependencies, 10, TimeSpan.FromSeconds(15))
          ));
    }

    class CustomExecutionStrategy : ExecutionStrategy
    {
      public CustomExecutionStrategy(ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay) : base(dependencies, maxRetryCount, maxRetryDelay)
      {
      }

      public CustomExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) : base(context, maxRetryCount, maxRetryDelay)
      {
      }

      int retryCount = 0;
      protected override bool ShouldRetryOn(Exception exception)
      {
          + Yeniden bağlantı durumunun söz konusu olduğu anlarda yapılacak işlemler...
          Console.WriteLine($"#{++retryCount}. Bağlantı tekrar kuruluyor...");
          return true;
      }
    }
*/
#endregion

#region needs to be able to play back each operation in a retry block that fails
/*

  * An execution strategy that automatically retries on failures needs to be able to play back each operation in a retry block that fails.
  * When retries are enabled, each operation you perform via EF Core becomes its own retriable operation.
  * That is, each query and each call to SaveChanges() will be retried as a unit if a transient failure occurs.

  * The solution is to manually invoke the execution strategy with a delegate representing everything that needs to be executed.
  * If a transient failure occurs, the execution strategy will invoke the delegate again.

  + EF Core ile yapılan çalışma sürecinde veritabanı bağlantısının kesildiği durumlarda, bazen bağlantının tekrardan kurulması tek başına yetmemektedir.
  + Kesintinin olduğu çalışmanın da baştan tekrardan işlenmesi gerekebilmetkedir.
  + İşte bu tarz durumlara karşılık EF Core Execute() - ExecuteAsync() fonksiyonlarını bizlere sunmaktadır.
  + Execute() fonksiyonu, içerisine verilen kodları commit edilene kadar işleyecektir.
  ! Bir bağlantı kesilmesi meydana gelirse, bağlantının tekrardan kurulması durumunda Execute içerisindeki çalışmalar tekrar baştan işlenecek ve böylece yapılan işlemin tutarlılığı için gerekli çalışma sağlanmış olacaktır.
 
    var strategy = _context.Database.CreateExecutionStrategy();

    await strategy.ExecuteAsync(async () =>
    {
      using var transcation = await _context.Database.BeginTransactionAsync();
      await _context.Persons.AddAsync(new() { Name = "David" });
      await _context.SaveChangesAsync();

      await _context.Persons.AddAsync(new Person() { Name = "Jane" });
      await _context.SaveChangesAsync();

      await _transcation.CommitAsync();
    });
*/
#endregion
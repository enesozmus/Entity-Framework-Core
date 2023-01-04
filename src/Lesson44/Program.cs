Console.WriteLine("Hello, World!");

#region Log
/**

  * * Detailed list of an application information, system performance, or user activities.
  * * A log can be useful for keeping track of computer use, emergency recovery, and application improvement.
  * * In a clear way, logging is just a fancy word to define a process of writing down everything you do.
  * * There are several kinds of logging.
  * ! You may log every operation of an application, log only when errors occur, or log crytical operations done by a user.
*/
#endregion

#region Loglama nedir?
/**

  * * Öncelikle log kelimesi kayıt anlamına gelmektedir.
  * * Loglama işlemi ise bu kayıtlar ile dijital hareketlerin tutulma işlemine denir.
  * * Gelişen teknoloji ile yazılımlar, işletim sistemleri, IOT cihazları gibi elektronik sitemler üzerinde yapılan her işin kayıtlarının tutulmasıdır.
  * * Log kayıtları tüm elektronik cihazlar için önemli ve siber güvenlik olaylarının aydınlatılması için de zorunlu hale getirilmiştir.
  * * Log kayıtları, sisteme giriş, çıkış, sistemde yapılan işlemler ve kullanıcının kim olduğuna dair tutulan kayıtlar olduğu için herhangi bir güvenlik sorunu oluştuğunda geriye dönük izlenme imkanı sunar.
  * * Çalışan bir sistemin runtime'da nasıl davranış gerçekleştirdiğini gözlemleyebilmek için log mekanizmalarından istifade ederiz.
  * *  
*/
#endregion

#region Simple Logging in Entity Framework Core
/**

  * * EF Core logs can be accessed from any type of application through the use of LogTo() when configuring a DbContext instance.
  * ! Adının 'simple' olması minumum yapılandırma gerektirmesi ve herhangi bir nuget paketine ihtiyaç duymamasından kaynaklanmasıdır.
  * * This configuration is commonly done in an override of DbContext.OnConfiguring. For example:

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.LogTo(Console.WriteLine);
      optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }
*/

#region Writing a log to an external file
/**

  * * Normalde Console yahut Debug pencerelerine atılan loglar pek takip edilebilir nitelikte olmamaktadır.
  * * Logları kalıcı hale getirmek istediğimiz durumlarda en basit haliyle harici bir dosyaya atmak isteyebiliriz.

    class ApplicationDbContext : DbContext
    {
      StreamWriter _log = new("logs.txt", append: true);

      protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message),LogLevel.Information)
                        .EnableSensitiveDataLogging() // Logging Sensitive Data
                        .EnableDetailedErrors();  // Exception Ayrıntısını Loglama
      }

      public override void Dispose()
      {
          base.Dispose();
          _log.Dispose();
      }

      public override async ValueTask DisposeAsync()
      {
          await base.DisposeAsync();
          await _log.DisposeAsync();
      }
    }
*/
#endregion


#region Logging Sensitive Data - EnableSensitiveDataLogging
/**

  * ! .EnableSensitiveDataLogging() ↑
  * * EF Core, default olarak log mesajlarında herhangi bir verinin değerini içermemektedir.
  * * Bunun nedeni, gizlilik teşkil edebilecek verilerin loglama sürecinde yanlışlıkla da olsa açığa çıkmamasını sağlamaktır.
  * * Fakat bazen alınan hatalarda verinin değerini bilmek hatayı debug edebilmek için oldukça yardımcı olabilmektedir.
  * * Bu durumda EF Core'un hassas verileri de loglamasını sağlayabiliriz.
*/
#endregion
#endregion

#region Query Log
/*

  * * LINQ soprguları neticesinde generate edilen sorguları izleyebilmek ve olası teknik hataları ayıklayabilmek amacıyla query log mekanizmasından istifade etmekteyiz.
  * * Microsoft.Extensions.Logging.Console

    class ApplicationDbContext : DbContext
    {

      readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
      
      ! filtreleme
      readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder
      .AddFilter((category, level) =>
      {
          return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
      })
      .AddConsole());
    }


    await _ontext.Persons
            .Include(p => p.Orders)
            .Where(p => p.Name.Contains("a"))
            .Select(p => new { p.Name, p.PersonId })
            .ToListAsync();
*/
#endregion
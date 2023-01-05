Console.WriteLine("Hello, World!");

#region Temporal Tables
/*

  * Yazılım Uzmanı olmanın temel ve olmazsa olmaz şartlarından biriside doğru bir raporlama ve takip mekanizması oluşturmaktan geçmektedir.
  * Temporal Tables özelliği, Zamansal Tablolar diye nitelendirebileceğimiz bir SQL Server 2016 yeniliğidir.
  * Bu özelliğin özeti, veritabanında yapılan Data Manipulation Language işlemlerini raporlamamızı sağlayan bir yapıdır.
  * https://www.gencayyildiz.com/blog/sql-server-2016-temporal-tables/
  
  ? Temporal Tables: Veri değişikliği süreçlerinde kayıtları depolayan ve zaman içinde farklı noktalardaki tablo verilerinin analizi için kullanılan ve sistem tarafından yönetilen tablolardır.
  ? EF Core 6.0 ile desteklenmektedir.
*/
#endregion


#region How to Work? - IsTemporal()
/*

  * Eldeki tablolar migration'lar aracılığıyla Temporal Table'lara dönüştürülüp veritabanı seviyesinde üretilebilmektedirler.
  ! Herhangi bir tablodaki bir verinin geçmişteki herhangi bir T anındaki hali/duırumu/verileri geri getirilebilmektedir.


  class Employee
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
  }

  class ApplicationDbContext : DbContext
  {
      public DbSet<Employee> Employees { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Employee>().ToTable("Employees", builder => builder.IsTemporal());
      }
  }
*/
#endregion


#region TemporalAsOf()
/*

  * Belirli bir zaman için değişikiğe uğrayan tüm öğeleri döndüren bir fonksiyondur.


    var datas = await context.Persons.TemporalAsOf(new DateTime(2022, 12, 09, 05, 30, 04)).Select(p => new
    {
      p.Id,
      p.Name,
      PeriodStart = EF.Property<DateTime>(p, "PeriodStart"),
      PeriodEnd = EF.Property<DateTime>(p, "PeriodEnd"),
    }).ToListAsync();

    foreach (var data in datas)
    {
      Console.WriteLine(data.Id + " " + data.Name + " | " + data.PeriodStart + " - " + data.PeriodEnd);
    }
*/
#endregion


#region TemporalAll()
/*

  * Güncellenmiş yahut silinmiş olan tüm verilerin geçmiş sürümlerini veya geçerli durumlarını döndüren bir fonksiyondur.

    var datas = await context.Persons.TemporalAll().Select(p => new
    {
      p.Id,
      p.Name,
      PeriodStart = EF.Property<DateTime>(p, "PeriodStart"),
      PeriodEnd = EF.Property<DateTime>(p, "PeriodEnd"),
    }).ToListAsync();

    foreach (var data in datas)
    {
      Console.WriteLine(data.Id + " " + data.Name + " | " + data.PeriodStart + " - " + data.PeriodEnd);
    }
*/
#endregion


#region TemporalFromTo()
/*

  * Belirli bir zaman aralığı içerisindelki verileri döndüren fonksiyondur. Başlangıç ve bitiş zamanı dahil değildir.


    //Başlangıç : 2022-12-09 05:29:55.0953716
    var baslangic = new DateTime(2022, 12, 09, 05, 29, 55);
    //Bitiş     : 2022-12-09 05:30:30.3459797
    var bitis = new DateTime(2022, 12, 09, 05, 30, 30);

    var datas = await context.Persons.TemporalFromTo(baslangic, bitis).Select(p => new
    {
      p.Id,
      p.Name,
      PeriodStart = EF.Property<DateTime>(p, "PeriodStart"),
      PeriodEnd = EF.Property<DateTime>(p, "PeriodEnd"),
    }).ToListAsync();

    foreach (var data in datas)
    {
      Console.WriteLine(data.Id + " " + data.Name + " | " + data.PeriodStart + " - " + data.PeriodEnd);
    }
*/
#endregion


#region TemporalBetween()
/*

  * Belirli bir zaman aralığı içerisindelki verileri döndüren fonksiyondur. Başlangıç verisi dahil değil ve bitiş zamanı ise dahildir.

    //Başlangıç : 2022-12-09 05:29:55.0953716
    var baslangic = new DateTime(2022, 12, 09, 05, 29, 55);
    //Bitiş     : 2022-12-09 05:30:30.3459797
    var bitis = new DateTime(2022, 12, 09, 05, 30, 30);

    var datas = await context.Persons.TemporalBetween(baslangic, bitis).Select(p => new
    {
      p.Id,
      p.Name,
      PeriodStart = EF.Property<DateTime>(p, "PeriodStart"),
      PeriodEnd = EF.Property<DateTime>(p, "PeriodEnd"),
    }).ToListAsync();

    foreach (var data in datas)
    {
      Console.WriteLine(data.Id + " " + data.Name + " | " + data.PeriodStart + " - " + data.PeriodEnd);
    }
*/
#endregion


#region TemporalContainedIn()
/*

  * Belirli bir zaman aralığı içerisindelki verileri döndüren fonksiyondur. Başlangıç ve bitiş zamanı ise dahildir.

  //Başlangıç : 2022-12-09 05:29:55.0953716
  var baslangic = new DateTime(2022, 12, 09, 05, 29, 55);
  //Bitiş     : 2022-12-09 05:30:30.3459797
  var bitis = new DateTime(2022, 12, 09, 05, 30, 30);

  var datas = await context.Persons.TemporalContainedIn(baslangic, bitis).Select(p => new
  {
    p.Id,
    p.Name,
    PeriodStart = EF.Property<DateTime>(p, "PeriodStart"),
    PeriodEnd = EF.Property<DateTime>(p, "PeriodEnd"),
  }).ToListAsync();

  foreach (var data in datas)
  {
    Console.WriteLine(data.Id + " " + data.Name + " | " + data.PeriodStart + " - " + data.PeriodEnd);
  }
*/
#endregion


#region Silinmiş Bir Veriyi Temporal Table'dan Geri Getirme
/*

  * Silinmiş bir veriyi temporal table'dan getirebilmek için öncelikle yapılması gerekenb ilgili verinin silindiği tarihi bulmamız gerekmektedir.
  * Ardından TemporalAsOf fonksiyonu ile silğinen verinin geçmiş değeri elde edilebilir ve fizilse tabloya bu veri taşınabilir.

  ? Silindiği tarih
    var dateOfDelete = await context.Persons.TemporalAll()
        .Where(p => p.Id == 3)
        .OrderByDescending(p => EF.Property<DateTime>(p, "PeriodEnd"))
        .Select(p => EF.Property<DateTime>(p, "PeriodEnd"))
        .FirstAsync();

    var deletedPerson = await context.Persons.TemporalAsOf(dateOfDelete.AddMilliseconds(-1))
        .FirstOrDefaultAsync(p => p.Id == 3);

    await context.AddAsync(deletedPerson);

    await context.Database.OpenConnectionAsync();

    await context.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Persons ON");
    await context.SaveChangesAsync();
    await context.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Persons OFF");
*/
#region SET IDENTITY_INSERT Konfigürasyonu
// Id ile veri ekleme sürecinde ilgili verinin id sütununa kayıt işleyebilmek için veriyi fiziksel tabloya taşıma işleminden önce veritabanı seviyesinde SET IDENTITY_INSERT komutu eşliğinde id bazlı veri ekleme işlemi aktifleştirilmelidir.
#endregion
#endregion
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


#region AsNoTracking()
/**

  * * Returns a new query where the entities returned will not be cached in the DbContext or ObjectContext.
  * * Returns a new query where the entities returned will not be cached in the DbContext or ObjectContext.
  * * Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.
  * * Dolayısıyla Change Tracker, takip ettiği nesnelerin sayısıyla doğru orantılı olacak şekilde bir maliyete sahiptir.
  * * Dolayısıyla üzerinde işlem yapılmayacak verilerin takip edilmesi bizlere gereksiz bir maliyet ortaya çıkaracaktır.
  * * AsNoTracking() metodu, context üzerinden sorgu neticesinde gelecek olan verilerin Change Tracker tarafından takip edilmesini engeller.
  * * AsNoTracking() metodu ile yapılan sorgulamalarda, verileri elde edebilir, bu verileri istenilen noktalarda kullanabilir lakin veriler üzerinde herhangi bir değişiklik/update işlemi yapamayız.
      
      - _context.Students.AsNoTracking().ToList();

  * * The change tracker will not track any of the entities that are returned from a LINQ query.
  * * If the entity instances are modified, this will not be detected by the change tracker and SaveChanges() will not persist those changes to the database.
  * * Disabling change tracking is useful for read-only scenarios because it avoids the overhead of setting up change tracking for each entity instance.
  * * You should not disable change tracking if you want to manipulate entity instances and persist those changes to the database using SaveChanges().

*/
#endregion


#region AsNoTrackingWithIdentityResolution()
/**

  * * Change Tracker mekanizması yinelenen verileri tekil instance olarak getirir. Bu bize bir performans kazancı sağlar.

      // Change Takip Mekanizması devrede
      // K    → A
      // K    ↑
      // K    ↑
      // K    ↑
      // K    ↑
      // K    → U 
      // K    ↑
      // K    ↑
      
  * * Change Tracker mekanizmasını AsNoTracking() metodu ile devreden çıkarırsak 'özellikle ilişkisel tabloları sorgularken' ekstra bir maliyete sebebiyet verebiliriz.
  * * AsNoTracking() ile elde edilen veriler takip edilmeyeceğinden dolayı yinelenen verilerin ayrı instancelarda olmasına sebebiyet veriyoruz.

      // Change Takip Mekanizması devrede değil
      // K    → A
      // K    → A
      // K    → A
      // K    → A
      // K    → U
      // K    → U

  * * Böyle bir durumda hem takip mekanizmasının maliyeitni ortadan kaldırmak hem de ilişkisel yinelenen dataları tek bir instance üzerinde karşılamak için AsNoTrackingWithIdentityResolution() fonksiyonunu kullanabiliriz.
  * * AsNoTrackingWithIdentityResolution() fonksiyonu AsNoTracking() fonksiyonuna nazaran görece daha yavaştır daha maliyetlidir.
  * * Fakat AsNoTrackingWithIdentityResolution() fonksiyonu, CT'a nazaran daha performanslı ve daha az maliyetlidir.

        - _context.Students.Include(x => x.Lesons)AsNoTrackingWithIdentityResolution().ToList();
        
*/
#endregion


#region UseQueryTrackingBehavior()
//Uygulama seviyesinde, EF Core seviyesinde ilgili context'ten gelen veriler üzerinde CT mekanizması davranışını temel seviyede belirlememizi sağlayan fonksiyondur.

// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// {
//   optionsBuilder.UseSqlServer("............");
//   optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
// }
#endregion


#region AsTracking()
// Context üzerinden gelen dataların CT tarafından takip edilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.
// UseQueryTrackingBehavior() ile kapattığımız CT mekanizmasını istediğimiz noktatalarda tekrar açabiliriz.
#endregion


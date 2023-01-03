Console.WriteLine("Hello, World!");

#region Creating and Using Views
/**

    * * Veritabanında birden fazla tabloyu JOIN yapısı ile birleştirip, istediğimiz kolonları getirmek için sorgular yazarız.
    * * Ancak elde ettiğimiz sorgu sonucunu ilerde tekrar kullanmak istediğimizde, aynı SQL sorgusunu tekrar yazmamız gerekmektedir.
    * * Bu nedenle, her ihtiyacımız olduğunda aynı SQL sorgusunu yazmamak için, View (sanal tablo) yapısı kullanılır.
    * * SQL'de View, bir veya daha fazla tablonun alanlarından oluşan sanal bir tablodur.
    * * Oluşturduğumuz View’lar veriyi içinde tutmaz. Her çağrıldığında sorguyu yeniden çalıştırır.
*/
#endregion

#region Kod Tarafinda View Oluşturma
/**

    * * Boş bir migration oluşturulmalıdır.
    * * Migration içerisindeki Up fonksiyonunda View'ın create komutları, Down fonksiyonunda ise drop komutları yazılmalıdır.

        - migrationBuilder.Sql($@"CREATE VIEW...Buraya SQL kodlarıyla serverda çalışacak View yazılır.");
        - migrationBuilder.Sql($@"DROP VIEW... View'ın adı");

    * * Migration migrate edilmelidir.
    * ! View'ı EF Core üzerinden sorgulayabilmek/karşılayabilmek için View kolonlarına/property'lerine uygun bir entity oluşturulması ve context'e DbSet<> olarak eklenmesi gerekmektedir.

        - public DbSet<PersonOrder> PersonOrders { get; set; }

    * * DbSet<>'in Bir View Olduğunu Bildirmek

        - modelBuilder.Entity<PersonOrder>()
                            .ToView("vm_PersonOrders")
                            .HasNoKey();

    * * View'ı kullanmak

        - var personOrders = await context.PersonOrders
                                    .Where(po => po.Count > 10)
                                    .ToListAsync();

    * ! View'lerde Primary Key olmamasına dikkat ederiz! Bu yüzden ilgili DbSet<>'in HasNoKey() ile işaretlenmesi gerekemktedir.
    * ! HasNoKey() → EF Core tarafından track edilmez.
*/
#endregion


#region Server'dan hazir View Kullanmak
/**

    * * View veritabanında yazılır ve kaydedilir.
    * * Bu View'i karşılayacak bir class model'ı oluşturulur. -public class Abcd {...}
    * * Bu model context'e DbSet<> olarak eklenir.
    * * OnModelCreating'e ToView("") yöntemi ile kaydı yapılır.

        - modelBuilder.Entity<T...>().HasNoKey().ToView("");

    * ! HasNoKey() → EF Core tarafından track edilmez.
*/
#endregion
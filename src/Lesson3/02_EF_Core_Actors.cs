/* Essential Concepts and Actors of Entity Framework Core */

/** DBContext <=> Veritabanı Nesnesi

  * * DbContext sınıfı, EF Core dünyasında hedef veritabanını temsil eder.
  * * Bu temsilin gerçekleşebilmesi için bu sınıfın 'Microsoft.EntityFrameworkCore' namespace'indeki DbContext sınıfından türetilmesi gerekmektedir.
  * * Bu sınıfın içerisinde veritabanındaki tabloları temsil eden Class'lar DbSet<TEntity> türünde tanımlanırlar.

  * * Yazılım ile veri tabanı arasındaki bağlantılar DbContext nesnesi üzerinden yürütülür.
  * * Veri tabanındaki tablolar ile yazılım nesneleri arasındaki iletişim DbContext nesnesi üzerinden yürütülür.
  * * Veri tabanı konfigurasyonları DbContext nesnesi üzerinden yürütülür.
  * * Verilerin sorgulanması, eklenmesi, güncellenmesi, silinmesi gibi veri tabanı işlemleri DbContext nesnesi üzerinden yürütülür.
  * * Mapping işlemleri DbContext nesnesi üzerinden yürütülür.
  * * Sorgulama neticesinde elde edilen verilerin takibi DbContext nesnesi üzerinden yürütülür.
  * * Change Tracking işlemi DbContext nesnesi üzerinden yürütülür.
  * * Caching DbContext nesnesi üzerinden yürütülür.

    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext { } 

*/


/** Entity <=> Tablo Nesnesi

  * * Veritabanındaki tabloları temsil eden sınıflar EF Core dünyasında Entity olarak nitelendirilirler.
  * * Birer Class olarak oluşturulurlar ve DbContext sınıfı içerisinde DbSet<TEntity> olarak işaretlenirler. 
  * * 
  * * Veritabanındaki tabloları temsil eden bu sınıflar kod tarafında tekil olarak isimlendirilirler.
*/


/** Property <=> Tablo Kolonu

  * * Entity sınıflarının içerisindeki property'ler ise tabloların sütunlarını temsil eder.


    public class Customer
    {
      public int ID {get; set;}
      public string Name {get; set;}
    }

*/


/** Datas <=> Veriler

  * * Veritabanındaki veriler ise entity'lerin instance'larına karşılık gelmektedir.
  * * new()

*/


/** Notes

  * ? Entity: Yeryüzündeki herhangi bir varlığın/olgunun/nesnenin özelliklerinin ve davranışlarının modellenebildiği sınıfa denir.
  * ? DbSet<TEntity> sınıfı, model içindeki belirli bir entity için bir koleksiyonu temsil eder ve bir varlığa karşı veritabanı işlemlerine açılan ağ geçididir.
  * ? Change Tracking: Context üzerinden gelen Entity'leri takip ederek Entity'ler üzerinde yapılan değişiklikleri algılar ve buna göre bir nesnenin EntityState'ini ayarlar.
  * ? Entity State: Bir entity instance'ının durumunu ifade eden bir referanstır.
  * ? Object Caching: DbContext, veri deposundan alması istenen nesneler için birinci düzey bir önbellek sağlar. Aynı nesne için sonraki istekler, başka bir veritabanı isteği yürütmek yerine önbelleğe alınan nesneyi döndürür.
  * ? Migration: Yazılım içeresinde yapmış olduğumuz modellemeyi veri tabanının anlayabileceği hale getiren bir işlemdir. Migrate ise Migration'ların oluşturulmasıdır.
  * ? Yaklaşım; bir konuyu, olguyu, yapıyı, inşayı, sorunu, çözümü ele alış bir başka deyişle bütünsel olarak bakış biçimidir.
*/
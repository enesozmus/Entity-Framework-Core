using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

#region Customizing Entity Configurations
/**

    * * Entity'ler için default davranışları geçersiz kılmak ya da özelleştirmek istediğinde bu yapılandırma ayarlarını kullanırız.
    * * Entity'lerimiz için söz konusu yapılandırmaları gerçekleştirebileceğimiz ilk yöntem OnModelCreating() methodunu kullanmaktır.
    * * Bu method base DbContext sınıfı içerisinde virtual olarak ayarlanmış bir metottur.

    * * GetEntityTypes: EF Core'da kullanılan entity'leri elde etmek, programatik olarak öğrenmek istiyorsak GetEntityTypes() methodunu kullanabiliriz.

        - var entities = modelBuilder.Model.GetEntityTypes();

*/
#endregion

#region Data Annotations & Fluent API

#region Table - ToTable
// Generate edilecek tablonun ismini belirlememizi sağlayan yapılandırmadır.
// Ef Core normal şartlarda generate edeceği tablonun adını DbSet property'sinden almaktadır.
// Eğer bunu özelleştirmek istiyorsak bir yapılandırma ayarı yapailiriz.
// Bunun için Table attribute'ünü ya da ToTable API'ını kullanabiliriz.
#endregion

#region Column - HasColumnName, HasColumnType, HasColumnOrder
// EF Core'da tabloların kolonları entity sınıfları içerisindeki property'lere karşılık gelmektedir. 
// Default olarak property'lerin adı kolon adıyken, türleri/tipleri kolon türleridir.
// Generate edilecek kolon isimlerine ve türlerine müdahale etmek istiyorsak bu konfigürasyon kullanılır.
#endregion

#region ForeignKey - HasForeignKey
// İlişkisel tablo tasarımlarında, bağımlı tabloda esas tabloya karşılık gelecek verilerin tutulduğu kolon foreign key adlandırılır.
// EF Core'da foreign key kolonu genellikle Entity Tanımlama kuralları gereği default yapılanmalarla oluşturulur.
// ForeignKey Data Annotations Attribute'unu direkt kullanabilirsiniz.
// Ancak Fluent API ile bu konfigürasyonu yapacaksanız iki entity arasındaki ilişkiyi de modellemeniz gerekmektedir.
// Aksi taktirde Fluent API üzerinde HasForeignKey fonksiyonunu kullanamazsınız!
#endregion

#region NotMapped - Ignore
// EF Core, entity sınıfları içerisindeki tüm property'leri default olarak modellenen tabloya kolon şeklinde migrate eder.
// Bazen bizler entity sınıfları içerisinde tabloda bir kolona karşılık gelmeyen property'ler tanımlamak isteyebiliriz.
// Bu property'lerin EF Core tarafından kolon olarak map edilmesini istemediğimizi bildirebilmek için NotMapped veya Ignore yapılandırmalarını kullanabiliriz.
#endregion

#region Key - HasKey
// EF Core'da, default convention olarak bir entity'nin içerisinde Id, ID, EntityId, EntityID vs. şeklinde tanımlanan tüm proeprty'lere varsayılan olarak 'primary key constraint' uygulanır.
// Key ya da HasKey yapılandırmalarıyla istediğinmiz herhangi bir proeprty'ye default convention dışında 'primary key constraint' uygulayabiliriz.
// EF Core'da bir entity içerisinde kesinlikle Primary Key'i temsil edecek olan property bulunmalıdır.
// Aksi taktirde EF Core migration olutşurken hata verecektir. Eğer ki tablonun Primary Key'i yoksa olmayacaksa bunun bildirilmesi gerekir. 
#endregion

#region Required - IsRequired
// Bir kolonun nullable olup olmayacağı bu yapılandırmayla belirlenebilir.
// EF Core'da bir property default oalrak not null şeklinde tanımlanır.
// Herhangi bir property'nin nullable olamsını istiyorsak türü üzerinde ?(nullable) operatörü ile bir bildirimde bulunabiliriz.
#endregion

#region MaxLenght | StringLength - HasMaxLength
// Bir kolonun maximum karakter sayısını belirlememizi sağlar.
#endregion

#region Precision - HasPrecision
// Küsüratlı sayılarda bir kesinlik belirtmemizi ve noktanın kaç haneli olabileceğini bildirmemizi sağlayan bir yapılandırmadır.
#endregion

#region Unicode - IsUnicode
// Kolon içerisinde unicode karakterler kullanılacaksa bu yapılandırma kullanılabilir.
#endregion

#region Comment - HasComment
// EF Core üzerinden oluşturulmuş olan veritabanı nesneleri üzerinde bir açıkalama/yorum eklemek istendiğinde kullanılır.
#endregion

#region InverseProperty
// İki entity arasında birden fazla ilişki varsa bu ilişkilerin hangi navigation property üzerinden olacağını ayarlamamızı sağlayan bir yapılandırmadır.
#endregion

#region Timestamp - IsRowVersion
// Belirli senaryolara göre bir satırdaki verinin üzerinde yapılan değişiklikleri tutarlılığı sağlayabilmek adına takip etmek istiyor olabiliriz.
// İşte bu takip bu yapılandırmayla sağlanabilir.
// Sonraki derslerle 'veri tutarlılığı' başlıklı bir ders yapıyor olacağız.
// → Versiyon mantığı
#endregion

#region ConcurrencyCheck - IsConcurrencyToken
// Sonraki derslerle 'veri tutarlılığı' başlıklı bir ders yapıyor olacağız.
// Bu derste bir satırdaki verinin bütünsel olarak tutarlılığını sağlayacak bir concurrency token yapılanmasından bahsececeğiz.
#endregion
#endregion

#region Data Annotations

[Table("Akdeniz")]
public class Person
{
  //[Key]
  public int Id { get; set; }
  //[ForeignKey(nameof(Department))]
  public int CustomizeId { get; set; }
  //[Column("Adi", TypeName = "metin", Order = 7)]
  public string Name { get; set; }
  //[Required()]
  //[MaxLength(13)]
  //[StringLength(14)]
  [Unicode]
  public string? Surname { get; set; }
  //[Precision(5, 3)]
  public decimal Salary { get; set; }
  //Yazılımsal amaçla oluşturduğum bir property
  //[NotMapped]
  //public string Laylaylom { get; set; }

  [Timestamp]
  //[Comment("Bu şuna yaramaktadır...")]
  public byte[] RowVersion { get; set; }

  //[ConcurrencyCheck]
  //public int ConcurrencyCheck { get; set; }

  public DateTime CreatedDate { get; set; }
  public Department Department { get; set; }
}
public class Department
{
  public int Id { get; set; }
  public string Name { get; set; }

  public ICollection<Person> Persons { get; set; }
}

public class Flight
{
  public int FlightID { get; set; }
  public int DepartureAirportId { get; set; }
  public int ArrivalAirportId { get; set; }
  public string Name { get; set; }
  public Airport DepartureAirport { get; set; }
  public Airport ArrivalAirport { get; set; }
}

public class Airport
{
  public int AirportID { get; set; }
  public string Name { get; set; }
  [InverseProperty(nameof(Flight.DepartureAirport))]
  public virtual ICollection<Flight> DepartingFlights { get; set; }

  [InverseProperty(nameof(Flight.ArrivalAirport))]
  public virtual ICollection<Flight> ArrivingFlights { get; set; }
}
#endregion

#region Fluent API
#region GetEntityTypes
//var entities = modelBuilder.Model.GetEntityTypes();
//foreach (var entity in entities)
//{
//    Console.WriteLine(entity.Name);
//}
#endregion
#region ToTable
//modelBuilder.Entity<Person>().ToTable("......");
#endregion
#region Column
//modelBuilder.Entity<Person>()
//    .Property(p => p.Name)
//    .HasColumnName(".....")
//    .HasColumnType(".....")
//    .HasColumnOrder(.....);
#endregion
#region ForeignKey
//modelBuilder.Entity<Person>()
//    .HasOne(p => p.Department)
//    .WithMany(d => d.Persons)
//    .HasForeignKey(p => p.CustomizeId);
#endregion
#region Ignore
//modelBuilder.Entity<Person>()
//    .Ignore(p => p.Laylaylom);
#endregion
#region Primary Key
//modelBuilder.Entity<Person>()
//    .HasKey(p => p.Id);
#endregion
#region IsRowVersion
//modelBuilder.Entity<Person>()
//    .Property(p => p.RowVersion)
//    .IsRowVersion();
#endregion
#region Required
//modelBuilder.Entity<Person>()
//    .Property(p => p.Surname).IsRequired();
#endregion
#region MaxLength
//modelBuilder.Entity<Person>()
//    .Property(p => p.Surname)
//    .HasMaxLength(13);
#endregion
#region Precision
//modelBuilder.Entity<Person>()
//    .Property(p => p.Salary)
//    .HasPrecision(5, 3);
#endregion
#region Unicode
//modelBuilder.Entity<Person>()
//    .Property(p => p.Surname)
//    .IsUnicode();
#endregion
#region Comment
//modelBuilder.Entity<Person>()
//        .HasComment("Bu tablo şuna yaramaktadır...")
//    .Property(p => p.Surname)
//        .HasComment("Bu kolon şuna yaramaktadır.");
#endregion
#region ConcurrencyCheck
//modelBuilder.Entity<Person>()
//    .Property(p => p.ConcurrencyCheck)
//    .IsConcurrencyToken();
#endregion
#endregion
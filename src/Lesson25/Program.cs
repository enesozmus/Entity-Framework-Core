using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

#region Generated Values Configuration
/**

    * * Generated Value: EF Core'da üretilen değerlerle ilgili çeşitli modellerin ayrıntılarını yapılandırmamızı sağlayan bir konfigürasyondur.
    * * Default Values: EF Core herhangi bir tablonun herhangi bir kolonuna yazılım tarafından bir değer gönderilmediği taktirde bu kolona hangi değerin(default value) üretilip yazdırılacağını belirleyen yapılanmalardır.
    * * Veritabanı sütunlarının değerleri çeşitli şekillerde oluşturulabilir:
    * * Primary key columns are frequently auto-incrementing integers
    * * Diğer sütunlar varsayılan veya hesaplanan değerlere sahiptir.

    * * Default values: On relational databases, a column can be configured with a default value; if a row is inserted without a value for that column, the default value will be used.

          modelBuilder.Entity<Blog>()
              .Property(b => b.Rating)
              .HasDefaultValue(3);
          modelBuilder.Entity<Blog>()
              .Property(b => b.Created)
              .HasDefaultValueSql("getdate()");

    * * Computed columns: On most relational databases, a column can be configured to have its value computed in the database, typically with an expression referring to other columns:

          modelBuilder.Entity<Person>()
              .Property(p => p.DisplayName)
              .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
              .HasComputedColumnSql("LEN([LastName]) + LEN([FirstName])", stored: true);

  * * Primary keys: By convention, non-composite primary keys of type short, int, long, or Guid are set up to have values generated for inserted entities if a value isn't provided by the application.
  * * Your database provider typically takes care of the necessary configuration; for example, a numeric primary key in SQL Server is automatically set up to be an IDENTITY column.
*/

/**

    * * Yukarıda EF Core'un birincil anahtarlar için değer oluşturmayı otomatik olarak kurduğunu gördük - ancak aynısını anahtar olmayan özellikler için de yapmak isteyebiliriz.

      public class Blog
      {
          public int BlogId { get; set; }
          public string Url { get; set; }

          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public DateTime Inserted { get; set; }
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Blog>()
              .Property(b => b.Inserted)
              .ValueGeneratedOnAdd();
      }

  * * Similarly, a property can be configured to have its value generated on add or update:

      public class Blog
      {
          public int BlogId { get; set; }
          public string Url { get; set; }

          [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
          public DateTime LastUpdated { get; set; }
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Blog>()
              .Property(b => b.LastUpdated)
              .ValueGeneratedOnAddOrUpdate();
      }

*/
#endregion

#region DatabaseGenerated

#region DatabaseGeneratedOption.None - ValueGeneratedNever
// EF Core'un Primary Key'ler için default olarak tanımladığı Identity özelliği kaldırılmak istendiğinde kullanılır..
#endregion

#region DatabaseGeneratedOption.Identity - ValueGeneratedOnAdd
// Herhangi bir kolona Identity özelliği tanımlayabilmemizi sağlayan bir yapılandırmadır.
/** Sayısal ve Sayısal Olmayan Türlerde **/
// Eğer ki Identity özelliği bir tabloda sayısal olan bir kolonda kullanılacaksa o durumda ilgili tablodaki pk olan kolondan özellikle/iradlei bir şekilde identity özelliğinin kaldırılması gerekmektedir.(None)
// Eğer ki Identity özelliği bir tabloda sayısal olmaan bir kolonda kullaınacaksa o durumda ilgili talbodaki pk olan kolondan iradeli bir şekilde identity özelliğinin kaldırılmasına gerek yoktur.
// Fakat bu kolonun nasıl artacağını EF Core'a özel olarak bildirmek gerekir.
// Primary Key ise artık kendiliğinde Identity özelliğini kaybetmiş olur.
#endregion

#region DatabaseGeneratedOption.Computed - ValueGeneratedOnAddOrUpdate
// EF Core üzerinde bir kolon Computed column ise ister Computed olarak belirleyebilirsiniz isterseniz de belirmeden kullanmaya devam edebilirsiniz.
#endregion
#endregion

#region HasDefaultValue()
// Static veri alır.
#endregion

#region HasDefaultValueSql()
// SQL ifadesi alır.
#endregion

#region HasComputedColumnSql()
// Tablo içerisindeki kolonlar üzerinde yapılan aritmatik işlemler neticesinde üretilen kolondur.
#endregion

#region Primary Keys
// Değer bilgisine null atanamayan ve unique olan property'lerdir.
#endregion

#region Identity
// Identity, yalnızca otomatik olarak artan bir property'dir. Manuel değer atanamaması durumudur.
// Bir property, Primary Key olmaksızın Identity olarak tanımlanabilir.
// Bir tablo içerisinde Identity kolonu sadece ve sadece bir adet tanımlanabilir.
// → Primary Key için → Identity özelliğini düşürmek gerekir  → [DatabaseGenerated(DatabaseGeneratedOption.None)]
// → tavsiye edilmez → PK sayısal türdeyse ilk ekleme defult 0 alır sonra manuel devam etmek gerekir
// → Identity özelliği eklemek için                           → [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

// Primary Key ve Identity unsurları genellikle birlikte kullanılmaktadırlar.
// EF Core Primary Key olan bir kolonu otomatik olarak Identity olacak şekilde yapılandırmaktadır.
#endregion


#region DbContext
class ApplicationDbContext : DbContext
{
  public DbSet<Person> Persons { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Person>()
        .Property(p => p.Salary)
        //.HasDefaultValue(100);
        .HasDefaultValueSql("FLOOR(RAND() * 1000)");

    modelBuilder.Entity<Person>()
        .Property(p => p.TotalGain)
        .HasComputedColumnSql("([Salary] + [Premium]) * 10")
        .ValueGeneratedOnAddOrUpdate();

    #region None
    modelBuilder.Entity<Person>()
        .Property(p => p.PersonId)
        .ValueGeneratedNever();
    #endregion

    #region Identity
    modelBuilder.Entity<Person>()
        .Property(p => p.PersonCode)
        .ValueGeneratedOnAdd();
    #endregion

    #region Primary Key'e None vermeden Sayisal olmayan türlerde Identity
    modelBuilder.Entity<Person>()
        .Property(p => p.PersonCode)
        .HasDefaultValueSql("NEWID()");
    #endregion
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer(".............");
  }
}

class Person
{
  //[DatabaseGenerated(DatabaseGeneratedOption.None)]
  public int PersonId { get; set; }
  public string Name { get; set; }
  public string Surname { get; set; }
  public int Premium { get; set; }
  public int Salary { get; set; }
  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public int TotalGain { get; set; }
  //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid PersonCode { get; set; }
}
#endregion
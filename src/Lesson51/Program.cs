Console.WriteLine("Hello, World!");

#region Concurrency Management
/*

  ? In most scenarios, databases are used concurrently by multiple application instances, each performing modifications to data independently of each other.
  ? When the same data gets modified at the same time, inconsistencies and data corruption can occur.
  * Concurrency conflicts occur when one user retrieves an entity's data in order to modify it, and then another user updates the same entity's data before the first user's changes are written to the database.
  * How you handle those conflicts depends on the nature of the changes being made.
  ! https://www.learnentityframeworkcore.com/concurrency

  + Geliştirdiğiniz yazılım uygulaması, eş zamanlı olarak son kullanıcılar tarafından yoğun bir işlevsel trafiğe maruz kalıyor ve bu yüzden veritabanı üzerinde fazlasıyla CRUD işlemleri söz konusu oluyorsa ‘veri tutarlılığı‘ sizler için oldukça önem arz ediyordur.
  + Uygulama, veritabanı aracılığıyla organize ettiği verileri en güncel ve en temiz haliyle son kullanıcıya sunabilmeli ve böylece ‘stale data’ yahut ‘dirty data’ şeklinde nitelendirilen işlevsiz verileri kullanıcıdan soyutlayabilmelidir.
  + Data Concurrency kavramı, uygulamalardaki olası veri tutarsızlığı durumlarınıın yönetilebilirliğini sağlayacak olan davranışları kapsayan bir kavramdır.
  ! https://www.gencayyildiz.com/blog/entity-framework-core-data-concurrency/

  + Stale Data: Veri tutarsızlığına sebebiyet verebilecek güncellenmemiş yahut zamanı geçmiş olan verileri ifade etmektedir.
  + Deadlock: Kitlenmiş olan bir verinin veirtabanı seviyesinde meydana gelen sistemsel bir hatadan dolayı kilidinin çözülememesi yahut döngüsel olarak kilitlenme durumunun meydana gelmesini ifade eden bir terimdir.
*/
#endregion


#region Last In Wins
/*

  * In many cases, there is only one version of the truth, so it doesn't matter if one user's changes overwrite another's changes.
  * In theory, the changes should result in the same update being made to the record.
  * For example, it doesn't matter if two users attempt to update a sports fixture record with the final score.
  * There is no need for any concurrency management strategy in this scenario.
  * This is known as the last in wins approach to concurrency control.
  + Bir veri yapısında son yapılan aksiyona göre en güncel verinin en üstte bulunmasını/varlığını korumasını ifade eden bir deyimsel terimdir.
*/
#endregion


#region Pessimistic Concurrency
/*

  * Pessimistic concurrency involves locking database records to prevent other users being able to access/change them until the lock is released.
  * However, the ability to lock records is not supported by all databases, and can be complex to program as well as highly resource intensive.
  * It is simply not practical at all in disconnected scenarios such as web applications.
  ! Entity Framework Core provides #no support# for pessimistic concurrency control.
*/
#endregion


#region Optimistic Concurrency
/*

  * Optimistic concurrency assumes that the update being made will be accepted, but prior to the change being made in the database, the original values of the record are compared to the existing row in the database and if any changes are detected, a concurrency exception is raised.
  * This is useful in situations where allowing one user's changes to overwrite another's could lead to data loss.
  * Entity Framework Core provides support for optimistic concurrency management.

  ? EF Core implements optimistic concurrency, which assumes that concurrency conflicts are relatively rare.
  ? In contrast to pessimistic approaches - which lock data up-front and only then proceed to modify it - optimistic concurrency takes no locks,
  ? ...but arranges for the data modification to fail on save if the data has changed since it was queried.
  ? This concurrency failure is reported to the application, which deals with it accordingly, possibly by retrying the entire operation on the new data.

  + Bir verinin stale olup olmadığını anlamak için herhangi bir locking işlemi olmaksızın versiyon mantığıonda çalışmamızı sağlayan yaklaşımdır.
  + Optimistic concurrency yönteminde, Pessimistic Concurrency'de olduğu gibi veriler üzerinde tutarsızlığa mahal olabilecek değişiklikler fiziksel olarak engellenmemektedir.
  + Bu yaklaşımla veriler üzerindeki tutarsızlık durumunu takip edebilmek için versiyon bilgis kullanılır.

      - Her bir veriye karşılık bir versiyon bilgisi üretilir.
      - Bu versiyon bilgisi veri üzerinde yapılan her bir değişiklik neticesinde güncellenir.
      - EF Core üzerinden verileri sorgularken ilgili verilerin versiyon bilgileri de in-memory'e alınır.
      - In-memory'e alınan veri güncellendiği takdirde in-memory'deki versiyon bilgisi ile veritabanındaki versiyon bilgisi karşılaştırılır.
      - Bu karşılaştırma doğrulanıyorsa yapılan aksiyon geçerli olacaktır, yok eğer doğrulanmıyorsa bir tutarsızlık durumu olduğu anlaşılacaktır.
      - İşte bu durumda bir hata fırlatılacak ve aksiyon gerçekleştirilmeyecektir.
  

  ! Entity Framework Core supports two approaches to concurrency conflict detection:

      1. configuring existing properties as concurrency tokens
      2. adding an additional "rowversion" property to act as a concurrency token
*/
#endregion


#region Property Based Configuration (ConcurrencyCheck Attribute)
/*

  * Properties can be configured as concurrency tokens via data annotations by applying the [ConcurrencyCheck] attribute:
  + Bu işaretleme neticesinde her bir entity'nin instance'ı için in-memory'de bir token değeri üretilecektir.
  + Üretilen bu token değeri alınan aksiyon süreçlerinde EF Core tarafından check edilecek herhangi bir farklılık yoksa doğrulanacak
  + ...ve aksiyon başarıyla sonlanacaktır.
  + [ConcurrencyCheck] attribute ile işaretlenmiş property'lerde herhangi  bir değişiklik durumu söz konusuysa üretilen token değiştirilecek
  + ...ve haliyle doğrulama sürecinde geçerli olmayacağı anlaşılacağı için veri tutarsızlığı durumu olduğu anlaşılacak ve hata fırlatılacaktır.

    public class Author
    {
      public int AuthorId { get; set; }
      public string FirstName { get; set; }
      
      [ConcurrencyCheck]
      public string LastName { get; set; }

      public ICollection<Book> Books { get; set; }
    }

  * Alternatively, properties can be configured using the Fluent API IsConcurrencyToken method:

  public class SampleContext : DbContext
  {
    public DbSet<Author> Authors { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
                          .Property(a => a.LastName)
                          .IsConcurrencyToken();
    } 
  }
  public class Author
  {
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Book> Books { get; set; }
  }
*/
#endregion


#region Adding a RowVersion property
/*

  * The second approach to concurrency management involves adding a column to the database table to store a version stamp for the row of data.
  * Different database systems approach this requirement in different ways.
  * SQL Server offers the rowversion data type for this purpose.
  * The column stores an incrementing number. Each time the data is inserted or modified, the number increments.

  public class Author
  {
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Book> Books { get; set; }
    [TimeStamp]
    public byte[] RowVersion { get; set; }
  }

* If you prefer to use the Fluent API to configure the property, you will use the IsRowVersion method:
  public class SampleContext : DbContext
  {
    public DbSet<Author> Authors { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Author>()
          .Property(a => a.RowVersion).IsRowVersion();
    } 
  }
  public class Author
  {
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Book> Books { get; set; }
    public byte[] RowVersion { get; set; }
  }
*/
#endregion
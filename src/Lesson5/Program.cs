using Microsoft.EntityFrameworkCore;


// DbContext
public class ExampleDbContext : DbContext
{
  /**

    * * Tipik olarak, DbContext'ten türetilen ve kod tarafında manuel olarak modellenen her bir entity için Microsoft.EntityFrameworkCore.DbSet property'lerini içeren bir class oluşturulur.
    * * Microsoft.EntityFrameworkCore.DbSet property'leri bir public setter'a sahipse, türetilmiş bağlamın instance'ı oluşturulduğunda bunlar otomatik olarak başlatılır.

  */
  public DbSet<Product> Products { get; set; }
  public DbSet<Customer> Customers { get; set; }


  /**

    * * DbContext sınıfı, method'un DbContextOptionsBuilder parametresi aracılığıyla context için yapılandırma bilgileri sağlayabilmeniz için geçersiz kılınmak üzere tasarlanmış virtual bir OnConfiguring method'una sahiptir.
    * * DbContext.OnConfiguring() method'u ezilerek/geçersiz kılınarak/override edilerek contex'in hangi sunucudaki veri tabanına bağlanacağını seçmemize (ve diğer seçenekleri yapılandırmamıza) olanak tanır.

    * * Microsoft.EntityFrameworkCore.DbContextOptionsBuilder'ı,
    
      1. DbContext.OnConfiguring(DbContextOptionsBuilder) method'unu override ederek ya da
	    2. harici olarak bir Microsoft.EntityFrameworkCore.DbContextOptions oluşturup context constructor'ına geçerek bir context'i yapılandırmak için kullanabilirsiniz.
  
  */

  /**
  
    * * public DbContextOptionsBuilder();
      
      -- Hiçbir seçenek/option ayarlanmadan Microsoft.EntityFrameworkCore.DbContextOptionsBuilder sınıfının yeni bir instance'ını başlatır.

    * * public DbContextOptionsBuilder(DbContextOptions options);

      -- Belirli bir Microsoft.EntityFrameworkCore.DbContextOptions'ı daha fazla yapılandırabileceğiniz bir instance'ını başlatır.

        + provider
        + ConnectionString
        + Lazy Loading 
        + etc.

  */
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    /**

      * * Connection String: Yazılım/uygulama ile veritabanı arasında gerekli olan bağlantının kurulabilmesi için gerekli olan hangi ana makineye bağlantı yapılacağını,
      o ana makinedeki hangi veritabanına bağlanacağımızı, o veritabanına bağlanmak için gerekli olan kullanıcı adı ve şifresi gibi bilgilerin tutulduğu bir kod parçasıdır.
      
    */
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=TestDb; Integrated Security=True;");
  }
}


// Entity
public class Product
{
  public int Id { get; set; }
  public string Name { get; set; }
  public int Quantity { get; set; }
  public float Price { get; set; }
}

// Entity
public class Customer
{
  public int Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
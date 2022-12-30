using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

Console.WriteLine("Hello, World!");

#region IEntityTypeConfiguration<T> Interface

#region OnModelCreating()
// Genel anlamda entity'ler üzerinde yapılandırma ayarları yapabildiğimiz bir fonksiyondur.
#endregion

#region IEntityTypeConfiguration<T>
// Entity bazlı yapılacak olan konfigürasyonları o entity'e özel harici bir dosya üzerinde yapmamızı sağlayan bir arayüzdür.
// Entity yapılandırma ayarlarının harici bir dosyada yürütülmesi merkezi bir yapılandırma noktası oluşturmamızı sağlamaktadır.
// Entity yapılandırma ayarlarının harici bir dosyada yürütülmesi entity sayısının fazla olduğu senaryolarda yönetilebilirliği artıracaktır.
#endregion

#region ApplyConfiguration()
// Bu metot harici yapılandırma sınıflarımızı EF Core'a bildirebilmek için kullandığımız bir metotdur.
#endregion

#region ApplyConfigurationsFromAssembly()
// Uygulama bazında oluşturulan harici yapılandırma sınıfların her birini OnModelCreating metodunda ApplyCOnfiguration ile tek tek bildirmek yerine
// ...bu sınıfların bulunduğu Assembly'i bildirerek IEntityTypeConfiguration arayüzünden türeyen tüm sınıfları ilgili entitye karşılık konfigürasyonel değer olarak baz almasını tek kalemde gerçekleştirmemizi sağlayan bir metottur.
#endregion

#endregion

class Order
{
  public int OrderId { get; set; }
  public string Description { get; set; }
  public DateTime OrderDate { get; set; }
}

class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.HasKey(x => x.OrderId);
    builder.Property(p => p.Description)
        .HasMaxLength(13);
    builder.Property(p => p.OrderDate)
        .HasDefaultValueSql("GETDATE()");
  }
}

class ApplicationDbContext : DbContext
{
  public DbSet<Order> Orders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //modelBuilder.ApplyConfiguration(new OrderConfiguration());      
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
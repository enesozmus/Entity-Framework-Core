using Microsoft.EntityFrameworkCore;


#region Adding data via the DbContext

/**

  * * Verilerin sorgulanması, eklenmesi, güncellenmesi, silinmesi gibi veritabanı işlemleri DbContext nesnesi üzerinden yürütülür.
  * * Dolayısıyla bu işlemleri gerçekleştirebilmek için DbContext nesnesinin instance'ına ihtiyaç duyulur.

      - ExampleDbContext context = new();

  * * Hedef veritabanında herhangi bir tabloya veri eklemek istersek bu işlem DbContext nesnesi üzerinden hedef tablonun bir instance'ıyla gerçekleşir.

      - Customer customer = new(){...};

  * * Nihayetinde gerçekleştirilmek istenen ekleme işlemi bir transaction eşliğinde veritabanına gönderilip execute edilmelidir.

      - SaveChangesAsync()
*/
ExampleDbContext _context = new();


Customer customer = new()
{
  // Id = 1,
  FirstName = "First",
  LastName = "Customer"
};


#region _context.Add and _context.AddAsync Methods

// _context.Add(customer);
// _context.Add<Customer>(customer);
// await _context.AddAsync(customer);
// await _context.AddAsync<Customer>(customer);

#endregion


#region _context.DbSet.Add and _context.DbSet.AddAsync Methods

// _context.Customers.Add(customer);
// await _context.Customers.AddAsync(customer);

#endregion

#region Adding Multiple Records

// _context.AddRange(customer1, customer2, customer3);
// await _context.AddRangeAsync(customer1, customer2, customer3);
// await _context.AddRangeAsync(customer, product);
// await _context.AddRangeAsync(customers);
// await _context.Customers.AddRangeAsync(customer1, customer2, customer3);

#endregion


#region _context.SaveChangesAsync() Method

/**

  * * Insert, update ve delete sorgularını oluşturup bir transaction eşliğinde veritabanına gönderip execute eden fonksiyodur.
  Eğer ki oluşturulan sorgulardan herhangi biri başarısız olursa tüm işlemleri geri alır.

  * * SaveChanges fonksiyonu her tetiklendiğinde bir transaction oluşituracağından dolayı EF Core ile yapılan her bir işleme özel kullanmaktan kaçınmalıyız!
  Çünkü her işleme özel transaction veritabanı açısından ekstradan maliyet demektir.

*/

// await _context.SaveChangesAsync();

#endregion


#region Entity States

/**
  * * Add'in herhangi bir sürümünü kullandığınızda context, method'a iletilen entity'i izlemeye başlar ve ona 'Added' adında bir EntityState değeri uygular.
  * ? Detached: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edilmediğini yani bağımsız/kendi halinde
  bir şekilde durduğunu söyler.
  * ? Added: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, henüz veritabanına eklenmediğini
  ancak eklenmek üzere işaretlendiğini söyler.
  * ? Unchanged: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, bu entity'nin veritabanında mevcut olduğunu
  ve değerlerinin veritabanındakilerle aynı olduğunu söyler.
  * ? Deleted: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, bu entity'nin veritabanında mevcut olduğunu
  ve silinmek üzere işaretlendiğini söyler.
  * ? Modified: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, bu entity'nin veritabanında mevcut olduğunu
  ve değerlerinin tamamının ya da bir kısmının veritabanındaki halinden artık farklı olduğunu söyler.


*/

Console.WriteLine("before AddAsync: " + _context.Entry(customer).State);
await _context.AddAsync<Customer>(customer);
Console.WriteLine("after AddAsync, before SaveChanges: " + _context.Entry(customer).State);
await _context.SaveChangesAsync();
Console.WriteLine("after SaveChanges: " + _context.Entry(customer).State);

#endregion

#endregion



// DbContext
public class ExampleDbContext : DbContext
{
  public DbSet<Customer> Customers { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=TestDb; Integrated Security=True;");
  }
}

public class Customer
{
  public int Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
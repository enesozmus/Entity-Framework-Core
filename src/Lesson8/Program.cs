using Microsoft.EntityFrameworkCore;


#region Deleting data via the DbContext

/**

  * * Verilerin sorgulanması, eklenmesi, güncellenmesi, silinmesi gibi veritabanı işlemleri DbContext nesnesi üzerinden yürütülür.
  * * Dolayısıyla bu işlemleri gerçekleştirebilmek için DbContext nesnesinin instance'ına ihtiyaç duyulur.

      - ExampleDbContext _context = new();

  * * Hedef veritabanında herhangi bir tablodaki herhangi bir veriyi silmek istersek öncelikle o veriye ulaşmalıyız.
  * * Silme işlemi ya context üzerinden erişilen instance üzerinden gerçekleştirilebilir ya da direkt instance üzerinden gerçekleştirilebilir.
  * * Hem veriye erişme hem de veriyi silme her iki işlem de DbContext nesnesi üzerinden yürütülür.

      - Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == 1);
      - Customer customer2 = await _context.Customers.FirstOrDefaultAsync(x => x.FirstName == "First");

      - _context.Customers.Remove(customer);

  * * Nihayetinde gerçekleştirilmek istenen silme işlemi bir transaction eşliğinde veritabanına gönderilip execute edilmelidir.

      - SaveChangesAsync()

*/
ExampleDbContext _context = new();

Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == 1);

_context.Customers.Remove(customer);
await _context.SaveChangesAsync();

#region ChangeTracker and Entity States

/**

  * * DbContext.Remove methodu, instance'ın EntityState'inin Deleted olarak ayarlar.

  * ? ChangeTracker: Context üzerinden gelen entity instance'larını default olarak yaşam süreleri boyunca takibinden sorumlu bir mekanizmadır. Bu takip neticesinde instance'ların EntityState'lerini ayarlar.
  * ? Entity State: Bir entity instance'ının durumunu ifade eden bir referanstır.

  * ? Deleted: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, bu entity'nin veritabanında mevcut olduğunu
  ve silinmek üzere işaretlendiğini söyler.

  * !  When SaveChanges is called, a DELETE statement is generated and executed by the database.

*/
#endregion


#region Disconnected Scenarios

// ID üzerinden
Customer customer2 = new()
{
  Id = 1,
  FirstName = "modified",
  LastName = "aaa"
};

// 1
_context.Customers.Remove(customer2);
// 2
context.Entry(customer2).State = EntityState.Deleted;
await _context.SaveChangesAsync();

#endregion


#region Deleting Multiple Records

// List<Customer> customers = await _context.Customers.ToListAsync();
// List<Customer> customers = await _context.Customers.Where(u => u.Id >= 7 && u.Id <= 9).ToListAsync();
// Customer customer7 = await _context.Customers.FirstOrDefaultAsync(x => x.Id == 7);

// _context.Customers.RemoveRange(customers);
// _context.Customers.RemoveRange(customer1, customer2, customer3);
// _context.RemoveRange(customers);
// _context.RemoveRange(customer7, customer8, customer9);
// await _context.SaveChangesAsync();

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

// Entity
public class Customer
{
  public int Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}

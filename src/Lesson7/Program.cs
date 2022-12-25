using Microsoft.EntityFrameworkCore;

#region Modifying data via the DbContext

/**

  * * Verilerin sorgulanması, eklenmesi, güncellenmesi, silinmesi gibi veritabanı işlemleri DbContext nesnesi üzerinden yürütülür.
  * * Dolayısıyla bu işlemleri gerçekleştirebilmek için DbContext nesnesinin instance'ına ihtiyaç duyulur.

      - ExampleDbContext _context = new();

  * * Hedef veritabanında herhangi bir tablodaki herhangi bir veriyi güncellemek istersek öncelikle o veriye ulaşmalıyız.
  * * Güncelleme işlemi ya context üzerinden erişilen instance üzerinden gerçekleştirilebilir ya da direkt instance üzerinden gerçekleştirilebilir.
  * * Hem veriye erişme hem de veriyi güncelleme her iki işlem de DbContext nesnesi üzerinden yürütülür.

      - Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == 1);
      - Customer customer2 = await _context.Customers.FirstOrDefaultAsync(x => x.FirstName == "First");
      - Console.WriteLine(customer.FirstName);

      - customer.Firstname = ".....";

  * * Nihayetinde gerçekleştirilmek istenen güncelleme işlemi bir transaction eşliğinde veritabanına gönderilip execute edilmelidir.

      - SaveChangesAsync()

*/
ExampleDbContext _context = new();

Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == 2);
Console.WriteLine("after FirstOrDefaultAsync: " + _context.Entry(customer).State);

customer.FirstName = "Second";
Console.WriteLine("after Second: " + _context.Entry(customer).State);
await _context.SaveChangesAsync();
Console.WriteLine("after SaveChangesAsync: " + _context.Entry(customer).State);

#endregion


#region ChangeTracker and Entity States

/**

  * * Herhangi bir entity instance'ı üzerinde kod tarafında bir değişiklik yaptığınızda context, o entity'i izlemeye başlar ve ona 'Modified' adında bir EntityState değeri uygular.

  * ? ChangeTracker: Context üzerinden gelen entity instance'larını default olarak yaşam süreleri boyunca takibinden sorumlu bir mekanizmadır. Bu takip neticesinde instance'ların EntityState'lerini ayarlar.
  * ? Entity State: Bir entity instance'ının durumunu ifade eden bir referanstır.

  * ? Unchanged: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, bu entity'nin veritabanında mevcut olduğunu
  ve değerlerinin veritabanındakilerle aynı olduğunu söyler.
  * ? Modified: Bir EntityState durumdur. Bize mevcut entity'nin Ef Core tarafından memory'de track edildiğini, bu entity'nin veritabanında mevcut olduğunu
  ve değerlerinin tamamının ya da bir kısmının veritabanındaki halinden artık farklı olduğunu söyler.

  * !  When SaveChanges is called, an UPDATE statement is generated and executed by the database.

*/
#endregion


#region  Disconnected Scenarios

/**

  * * Güncellemek istediğimiz veri context'ten gelmiyorsa direkt entity'nin instance'ı üzerinden güncellemek istiyorsak

      1. using the DbContext.Update method
      2. setting the EntityState for the entity explicitly
*/

// An itibarıyla bu instance'ın veritabanındaki verilerle bir ilişkisi yoktur. Bir eşlenme söz konusu değildir.
// Context üzerinden gelmediği için ChangeTracker da devrede değildir.
Customer customer2 = new()
{
  Id = 1,
  FirstName = "modified",
  LastName = "aaa"
};

#region 1. using the DbContext.Update method

// ChangeTracker mekanizması tarafından takip edilmeyen nesnelerin güncellenebilmesi için Update veya UpdateRange fonksiyonu kullanılır!
// Bu fonksiyonunu kullanabilmek için kesinlikle ilgili nesneye Id değeri verilmelidir!
// _context.Customers.Update(customer2);
// await _context.SaveChangesAsync();

#endregion

#region 2. setting the EntityState for the entity explicitly

// Bir entity instance'ının EntityState'ini, DbContext.Entry() method'u ile kullanılabilen EntityEntry.State özelliği aracılığıyla ayarlayabilirsiniz.

// _context.Entry(customer2).State = EntityState.Modified;
// _context.Entry(customer2).Property("FirstName").IsModified = true;
// Console.WriteLine("aaa: " + _context.Entry(customer2).State);

// ! When you use the Attach method on an entity, it's state will be set to Unchanged, which will result in no database commands being generated at all. 

#endregion

#endregion


#region Modifying Multiple Records

var customers = await _context.Customers.ToListAsync();

foreach (var customer in customers)
{
  customer.FirstName += "QQQ";
}
await _context.SaveChangesAsync();

// _context.UpdateRange(modifiedStudents);
// _context.UpdateRange(modifiedStudent1, modifiedStudent2, modifiedStudent3);
// _context.Students.UpdateRange(modifiedStudents);
// _context.Students.UpdateRange(modifiedStudent1, modifiedStudent2, modifiedStudent3);
// _context.SaveChangesAsync();

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
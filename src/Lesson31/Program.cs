using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Table-per-concrete-type
/**

    * * The table-per-concrete-type (TPC) feature was introduced in EF Core 7.0.
    * * In the TPC mapping pattern, all the types are mapped to individual tables.
    * * Each table contains columns for all properties on the corresponding entity type.
    * * This addresses some common performance issues with the TPT strategy.

    * * Kalıtımsal hiyerarşideki sadece 'concrete' sınıflara karşılık birer tablo oluşturmakta ve abstract yapılanmaların member'larını bu tablolara eklemektedir.
    * * TPC, TPT'nin daha performanslı versiyonudur.

    * * Her somut tür için bir tablo...
    * * Abstract olan sınıf üzerinde UseTpcMappingStrategy() fonksiyonu eşliğinde davranışın TPC olacağını belirleyebiliriz.

        - modelBuilder.Entity<Person>().UseTpcMappingStrategy();

*/
#endregion

#region TPC'de Veri Ekleme
// Davranışların hiçbirinde veri eklerken,silerken, güncellerken vs. normal operasyonların dışında bir işlem yapılmaz!

// Technician technician = new() { Name = "Soraka", Surname = "Lol", Department = "T-Heros", Branch = "Sup" };
// await _context.Technicians.AddAsync(technician);
#endregion

#region TPC'de Veri Silme
// Technician? technician = await _context.Technicians.FindAsync(1);
// _context.Employees.Remove(technician);
#endregion

#region TPC'de Veri Güncelleme
// Employee? guncellenecek = await _context.Employees.FindAsync(6);
// guncellenecek.Name = "XXXX...";
#endregion

#region TPC'de Veri Sorgulama
// var employees = await _context.Employees.ToListAsync();
// var techs = await _context.Technicians.ToListAsync();
#endregion


abstract class Person
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Surname { get; set; }
}
class Employee : Person
{
  public string? Department { get; set; }
}
class Customer : Person
{
  public string? CompanyName { get; set; }
}
class Technician : Employee
{
  public string? Branch { get; set; }
}

class Lesson31DbDbContext : DbContext
{
  public DbSet<Person> Persons { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Technician> Technicians { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // EF Core 7
    // modelBuilder.Entity<Person>().UseTpcMappingStrategy();
  }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=Lesson31Db; Integrated Security=True;");
  }
}
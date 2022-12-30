using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Table-per-type
/**

    * * Entity'lerin aralarında kalıtımsal ilişkiye sahip olduğu durumlarda her bir entity'e karşılık bir tablo generate eden davranıştır.
    * * Generate edilen bu tablolar hiyerarşik düzlemde kendi aralarında birebir ilişkiye sahiptir.
    * * Hiyerarşik olarak aralarında kalıtımsal ilişki olan tüm entity'ler OnModelCreating() fonksiyonunda ToTable() metodu ile konfigüre edilmelidir.
    * * Böylece EF Core kalıtımsal ilişki olan bu tablolar arasında TPT davranışının olduğunu anlayacaktır.

          modelBuilder.Entity<Person>().ToTable("Persons");
          modelBuilder.Entity<Employee>().ToTable("Employees");
          modelBuilder.Entity<Customer>().ToTable("Customers");
          modelBuilder.Entity<Technician>().ToTable("Technicians");

*/
#endregion

#region TPT'de Veri Ekleme
// Davranışların hiçbirinde veri eklerken,silerken, güncellerken vs. normal operasyonların dışında bir işlem yapılmaz!

// Technician technician = new() { Name = "Soraka", Surname = "Lol", Department = "T-Heros", Branch = "Sup" };
// await _context.Technicians.AddAsync(technician);
#endregion

#region TPT'de Veri Silme
// Technician? technician = await _context.Technicians.FindAsync(1);
// _context.Employees.Remove(technician);
#endregion

#region TPT'de Veri Güncelleme
// Employee? guncellenecek = await _context.Employees.FindAsync(6);
// guncellenecek.Name = "XXXX...";
#endregion

#region TPT'de Veri Sorgulama
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

class Lesson30DbContext : DbContext
{
  public DbSet<Person> Persons { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Technician> Technicians { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Person>().ToTable("Persons");
    modelBuilder.Entity<Employee>().ToTable("Employees");
    modelBuilder.Entity<Customer>().ToTable("Customers");
    modelBuilder.Entity<Technician>().ToTable("Technicians");
  }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=Lesson30Db; Integrated Security=True;");
  }
}
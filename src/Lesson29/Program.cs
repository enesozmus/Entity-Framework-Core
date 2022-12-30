using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Table-per-hierarchy
/**

    * * Persons adında tek bir tablo oluşturur.
    * * Bu tablo içerisinde otomatik olarak ayrıca bir [Discriminator] property'si/kolonu oluşturur.

      CREATE TABLE [dbo].[Persons](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](max) NULL,
        [Surname] [nvarchar](max) NULL,
        [Discriminator] [nvarchar](max) NOT NULL,
        [A] [int] NULL,
        [CompanyName] [nvarchar](max) NULL,
        [Department] [nvarchar](max) NULL,
        [Technician_A] [int] NULL,
        [Branch] [nvarchar](max) NULL,
      CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED 
      (
        [Id] ASC
      )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
      ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

*/
#endregion

#region TPH'da Veri Ekleme
// Davranışların hiçbirinde veri eklerken,silerken, güncellerken vs. normal operasyonların dışında bir işlem yapılmaz!

// Employee e1 = new() { Name = "David", Surname = "Lol", Department = "E-Heros" };
// Employee e2 = new() { Name = "Jane", Surname = "Lol", Department = "E-Heros" };
// Customer c1 = new() { Name = "Kaisa", Surname = "Lol", CompanyName = "C-Heros" };
// Customer c2 = new() { Name = "Yasuo", Surname = "Lol", CompanyName = "C-Heros" };
// Technician t1 = new() { Name = "Soraka", Surname = "Lol", Department = "T-Heros", Branch = "Sup" };

// await _context.Employees.AddAsync(e1);
// await _context.Employees.AddAsync(e2);
// await _context.Customers.AddAsync(c1);
// await _context.Customers.AddAsync(c2);
// await _context.Technicians.AddAsync(t1);
#endregion

#region TPH'da Veri Silme
// TPH davranışında silme operasyonu yine entity üzerinden gerçekleştirilir.

// Employee? employee = await _context.Employees.FindAsync(1);
// _context.Employees.Remove(employee);

// var customers = await _context.Customers.ToListAsync();
// _context.Customers.RemoveRange(customers);
#endregion

#region TPH'da Veri Güncelleme
// TPH davranışında güncelleme operasyonu yine entity üzerinden gerçekleştirilir.

// Employee? guncellenecek = await _context.Employees.FindAsync(6);
// guncellenecek.Name = "XXXX...";
#endregion

#region TPH'da Veri Sorgulama
// Kalıtımsal ilişkiye göre yapılan sorgulamada üst sınıf alt sınıftaki verileri de kapsamaktadır.
// Dolayısıyla üst sınıfların sorgulamalarında alt sınıfların verileri de gelecektir buna dikkat edilmelidir.

//var employees = await _context.Employees.ToListAsync();
//var techs = await _context.Technicians.ToListAsync();
#endregion

#region Aynı Entity'ler de Aynı İsimde Sütunların Olduğu Durumlar
// Entityler'de mükerrer kolonlar olabilir. Bu kolonları EF core isimsel olarak özelleştirip ayıracaktır.
// ? public int A { get; set; }
#endregion


class Person
// abstract class Person
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
  public int A { get; set; }
  public string? CompanyName { get; set; }
}
class Technician : Employee
{
  public int A { get; set; }
  public string? Branch { get; set; }
}

class Lesson29DbContext : DbContext
{
  public DbSet<Person> Persons { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Technician> Technicians { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Person>()
    // [Discriminator] property'sinin adını değiştirme
       .HasDiscriminator<string>("ayirici")
    // Adlarını direkt entity sınıflarından alan [Discriminator] değerlerini değiştirme
       .HasValue<Person>("isteAAAolsun")
       .HasValue<Employee>("isteBBBolsun")
       .HasValue<Customer>("isteCCColsun")
       .HasValue<Technician>("isteDDDolsun");
  }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=Lesson29Db; Integrated Security=True;");
  }
}
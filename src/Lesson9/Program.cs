using Microsoft.EntityFrameworkCore;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


#region Example: Using IEnumerable Interface

/**

  * * Aşağıdaki SQL Komut Dosyasında gösterildiği gibi, TOP yan tümcesi kullanılmayacaktır.
 
        1. IEnumerable, veritabanındaki verileri sorgularken "SELECT" deyimini yürütür
        2. ve SQL Server'dan verileri belleğe yükler
        3. filtreleri 'belleğe alınan verilere' uygular.

  SELECT 
    [Extent1].[ID] AS [ID], 
    [Extent1].[FirstName] AS [FirstName], 
    [Extent1].[LastName] AS [LastName], 
    [Extent1].[Gender] AS [Gender]
    FROM [dbo].[Student] AS [Extent1]
    WHERE 'Male' = [Extent1].[Gender]

*/

// SchoolDbContext _context = new();
// IEnumerable<Student> listStudents = _context.Students.Where(x => x.Gender == "Male");
// listStudents = listStudents.Take(2);

// foreach (var std in listStudents)
// {
//   Console.WriteLine(std.FirstName + " " + std.LastName);
// }
// Console.ReadKey();

#endregion


#region Example: Using IQueryable Interface

/**

  * * Aşağıdaki SQL Komut Dosyasında gösterildiği gibi, TOP yan tümcesi kullanılır ve ardından verileri veritabanından alır.
  * * When we use IQueryable, the actual data processing happens in the data store.

  SELECT 
    [Extent1].[ID] AS [ID], 
    [Extent1].[FirstName] AS [FirstName], 
    [Extent1].[LastName] AS [LastName], 
    [Extent1].[Gender] AS [Gender]
    FROM [dbo].[Student] AS [Extent1]
    WHERE 'Male' = [Extent1].[Gender]

*/

// SchoolDbContext _context = new();
// IQueryable<Student> listStudents = _context.Students.AsQueryable().Where(x => x.Gender == "Male");
// listStudents = listStudents.Take(2);

// foreach (var std in listStudents)
// {
//   Console.WriteLine(std.FirstName + " " + std.LastName);
// }
// Console.ReadKey();

#endregion


// DbContext
public class SchoolDbContext : DbContext
{
  public DbSet<Student> Students { get; set; }
  public DbSet<Lesson> Lessons { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Data Source=DESKTOP-OPFJQHD; Database=SchoolDb; Integrated Security=True;");
  }

  #region Seed Datas
  // INSERT INTO Students VALUES ('Steve', 'Smith', 'Male')
  // INSERT INTO Students VALUES ('Sara', 'Pound', 'Female')
  // INSERT INTO Students VALUES ('Ben', 'Stokes', 'Male')
  // INSERT INTO Students VALUES ('Jos', 'Butler', 'Male')
  // INSERT INTO Students VALUES ('Pam', 'Semi', 'Female')
  #endregion
}

// Entity 1
public class Student
{
  public int ID { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Gender { get; set; }

  public ICollection<Lesson> Lessons { get; set; }
}
// Entity 2
public class Lesson
{
  public int ID { get; set; }
  public string LessonName { get; set; }
}
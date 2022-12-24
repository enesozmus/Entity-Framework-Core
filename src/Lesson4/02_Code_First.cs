/** Code First

  * * Code First yaklaşımı veritabanını kod tarafında modelleyerek ardından bu modele uygun veritabanını sunucuda oluşturan yaklaşımdır. 
  * * Bu modelleme kabaca hedef veritabanını temsil edecek olan bir DbContext sınıfını, veritabanındaki tabloları temsil edecek olan Class'ları ve tabloların içindeki sütunları temsil edecek Property'leri kod tarafında manuel olarak yazmayı gerektirir.
  * * Dolayısıyla veri tabanının kod üzerinden modellenebilmesini sağlar.

*/


/**

  * * Oluşturmak istediğimiz veri tabanı tablolarına karşılık gelen Class'lar yazılır.
  * * Oluşturmak istediğimiz veri tabanına karşılık gelen bir DbContext Class'ı yazılır ve burada Tablolara karşılık gelen Class'lar referans edilirler.
  * * Manuel olarak yapılan bu modellemenin veritabanına göç edebilme yeteneği kazanabilmesi için C# migration sınıfı haline dönüştürülmesi gerekir.
  * ? Migration: İçerisinde Up ve Down adlarında iki adet fonksiyon barındıran ve yazılım içeresinde yapmış olduğumuz modellemeyi veri tabanı sunucusunun anlayabileceği hale, C# migration sınıfına, dönüştüren bir işlemdir.
  * ? Migrate ise Migration'ların oluşturulması, göç ettirilmesi nihahetinde veri tabanının oluşturulmasıdır.

    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext { } 

*/


/**

  * * Bir migration oluşturabilmek için aşağıdaki kütüphanelerin projeye entegre edilmesi gerekir.
      - dotnet add package Microsoft.EntityFrameworkCore --version 6.0.12
      - dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.12

  * * Bir migration oluşturabilmek için temelde EF Core aktörleri olan DbContext ve Entity class'larını oluşturmak gerekir.
      - dotnet add package Microsoft.EntityFrameworkCore --version 6.0.12

  * * Ardından PMC veya Dotnet CLI ile migration ve migrate komutları verilir.
      
      - add-migration [name]
      - update-database

      - dotnet ef migrations add [name]
      - dotnet ef database update
      
      - dotnet ef database update [name]

      - dotnet ef migrations add [name] --output-dir [path]
      - dotnet ef migrations list
      - dotnet ef migrations remove [name]
      
*/
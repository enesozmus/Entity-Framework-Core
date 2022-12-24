/** Database First

  * * Database First yaklaşımı hedef veritabanının belirli talimatlar aracılığıyla(scaffold) otomatik olarak kod kısmına OOP imkanları eşliğinde modellenmesidir.
  * * Bu modelleme kabaca hedef veritabanını temsil eden bir DbContext sınıfı, veritabanındaki tabloları temsil eden birçok Class ve tabloların içindeki sütunları temsil eden birçok Property oluşturur.
  * * EF Core ile çalışma yapılacak olan veritabanı önceden oluşturulmuş ise Database First yaklaşımı tercih edilmelidir.

*/

/** Reverse Engineering

  * * Reverse Engineering: Bir sunucudaki veritabanı iskelesinin kod kısmında oluşturulmasıdır.
  * * Bu süreç Package Manager Console ya da Dotnet CLI aracılığıyla gerçekleştirilebilmektedir.
  * * Bu süreç Package Manager Console kullanımında 'Scaffold-DbContext' komutuyla gerçekleştirebilir.
  * * Bu süreç Dotnet CLI kullanımında ise 'dotnet ef dbcontext scaffold' komutuyla gerçekleştirebilir.
  * * Yazılım ve veritabanı arasındaki bağlantı 'Connection String' üzerinden kurulur.
*/

/**

  * * PMC ile veritabanı modelleyebilmek için aşağıdaki kütüphanelerin projeye yüklenmesi gerekmektedir.

    1. Microsoft.EntityFrameworkCore.Tools
    2. DatabaseProvider (örn: Microsoft.EntityFrameworkCore.SqlServer)

    Scaffold-DbContext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook' Microsoft.EntityFrameworkCore.SqlServer
    
*/

/**

  * * Dotnet CLI ile veritabanı modelleyebilmek için aşağıdaki kütüphanelerin projeye yüklenmesi gerekmektedir.

    1. Microsoft.EntityFrameworkCore.Design
    2. DatabaseProvider (örn: Microsoft.EntityFrameworkCore.SqlServer)


    dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.12
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.12
    dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook" Microsoft.EntityFrameworkCore.SqlServer
    dotnet ef dbcontext scaffold "Data Source=DESKTOP-OPFJQHD; Database=Northwind; Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer

    dotnet user-secrets init
    dotnet user-secrets set ConnectionStrings:Chinook "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook"
    dotnet ef dbcontext scaffold Name=ConnectionStrings:Chinook Microsoft.EntityFrameworkCore.SqlServer
    
*/

/**

  * * Sadece istenilen tabloları getirme

    - dotnet ef dbcontext scaffold ... --table Artist --table Album

  * * DbContext adını belirtme

    - dotnet ef dbcontext scaffold ... --context ContextName

  * * Path ve Namespace belirtme
    
    - dotnet ef dbcontext scaffold ... --context-dir Data --output-dir Models
    - dotnet ef dbcontext scaffold ... --namespace Example.Entities --context-namespace Example.Contexts

  * * Model güncelleme

    - ... --force (!)

  * * finally
  
    - dotnet ef dbcontext scaffold "Data Source=DESKTOP-OPFJQHD; Database=Northwind; Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer --table Orders --table Products --context-dir Data --output-dir Models

*/
using Lesson1;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;


#region What is database?
/**
    * Bilgisayar kullanımında çözüme erişmek için işlenebilir duruma getirilmiş bilgi ortamı.
    * Veri tabanları birçok uygulamaya hizmet verebilen, bilgi depolayabilen, bilgisayar temelli kayıt tutma sistemleridir, yazılımlardır.
*/
#endregion


#region What is SQL?
/**
    * SQL stands for Structured Query Language.
    * SQL is a standard language for storing, accessing, manipulating and retrieving data in databases.
*/
#endregion


#region What is the relationship between databases and software?
/**
    * Yazılım uygulamalarında veriler fiziksel olarak veri tabanlarında tutulmaktadır.
    * Dolayısıyla kabaca verilerin işlenebilmesi ve erişilebilir olması için yazılım ile veri tabanları arasında sürekli faal olacak bir bağlantı sağlanmalıdır.
    * Bu bağlantı üzerinden yazılım ve veri tabanı arasında sürekli verisel bir trafik, iletişim ve veri alışverişi olmaktadır.
    * Bu alışveriş yazılımın veri tabanlarının anlayacağı dilden sorgular göndermesiyle gerçekleşmektedir.
    * Bu sorgular genel olarak SQL dilindedir. Örnek olaran bir veri tabanından verileri select etmek için 'SELECT * FROM table_name;' ifadesi kullanılır.
*/
#endregion


#region SQL into the code

await using SqlConnection connection = new($"Data Source=DESKTOP-OPFJQHD; Database=Northwind; Integrated Security=True;");
await connection.OpenAsync();

#region the disadvantages of writing SQL in code
/**
    * Kod içerisinde SQL sorguları yazılmıştır. Bu mutlak bir SQL bağımlılığı yaratır.
    * Kodun içerisine metinsel SQL ifadeler yazılması ve veri tabanından gelen sonuçların manuel olarak parse edilmesi büyük projelerde büyük problemler ortaya çıkarabilmektedir.
    * Dolayısıyla geliştirme ve bakım maliyeti yüksek kod inşasına sebep olur.
    * Zamanla hızla artacak metinsel ifadeler dolayısıyla yazılımın SQL'e olan bağımlılığı artacak ve kodun yönetilebilirliği zamanla ortadan kaybolacaktır.
    * ORM yaklaşımını kullanmadan kodun içerisine gömülen SQL ifadelerinin ilerideki dönüşüm maliyetini öngöremiyorsanız bu işin karar mekanizmalarında zaten olmayın. [Gençay Yıldız]
*/
#endregion

SqlCommand command = new("Select * from Employees", connection);
SqlDataReader dr = await command.ExecuteReaderAsync();
while (await dr.ReadAsync())
{
    Console.WriteLine($"{dr["FirstName"]} {dr["LastName"]}");
}
await connection.CloseAsync();

#endregion


#region What is ORM?
/** Nesne İlişkisel Eşleme

    * Nesne yönelimli yazılımlar ile veri tabanı arasındaki verisel trafiğin nesne yönelimli araçlarlın kullanımına izin verecek şekilde
    ve  SQL'e olan bağımlılığı azaltarak yönetilmesini öngören bir yaklaşımdır.

    * Nesne-ilişkisel eşleme, geliştiricilerin  SQL komutlarına ihtiyaç duymadan, bir uygulamanın programlama dilinde tanımlanan nesneler ile
    ilişkisel veri kaynaklarında depolanan veriler arasında eşleme yapmak için gereken işi gerçekleştirerek, verilerle nesne yönelimli bir şekilde
    çalışmasını sağlayan bir yaklaşımdır.

    * Dolayısıyla nesne-ilişkisel eşleme katı ve kompleks veri tabanı sorguları yerine veri tabanınızdaki tabloları Class’lara, kolonları Property’lere,
    tabloların içindeki kayıtları da Object’lere dönüştüren ve tüm bu dönüşün sonucunda oluşan Class’lar ve objeler üzerinden veri tabanı işlemlerinizi
    yapmayı sağlayan bir yaklaşımdır.

    * Bu sayede bütün veri tabanı süreçleri OOP kavramlarıyla yürütülebilir.

    * ORM yaklaşımını kullanmadan kodun içerisine gömülen SQL ifadelerinin ilerideki dönüşüm maliyetini öngöremiyorsanız bu işin karar mekanizmalarında zaten olmayın. [Gençay Yıldız]

    1. Veri tabanı tablolarına karşılık gelen Class'lar yazılır.
    2. Veri tabanına karşılık gelen bir Class yazılır ve burada Tablolara karşılık gelen Class'lar referans edilirler.

    SELECT * FROM Customers;

    _context.Customers.ToListAsync();

*/

NorthwindDbContext _context = new();
var employeeDatas = await _context.Employees.ToListAsync();

#endregion
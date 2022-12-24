/* Entity Framework
  * Microsoft tarafından
  * 2008 yılında
  * .NET Framework için
  * yazılımdaki nesneleri
  * ilişkisel bir veritabanının tablo ve sütunlarına eşlemeyi basitleştiren
  * bir Nesne İlişkisel Eşleştiricisi olarak piyasaya sürüldü.

  * Entity Framework, veritabanındaki verileri okumak veya yazmak için gerekli veritabanı komutlarını oluşturabilir ve bunları sizin için yürütebilir.
  * Kod içerisinde nesne yönelimli imkanlardan/araçlardan istifade edebilmemizi sağlar.
  * Code First ve Database First yaklaşımları eşliğinde yazılım ve veritabanı arasındaki koordinasyonu sağlamaktadır.
  * Dolayısıyla Entity Framework, ORM yaklaşımını benimsemiş araçlardan bir tanesidir.
*/

/* Entity Framework Core
  * yine Microsoft tarafından
  * 2016 yılında
  * .NET Core platformu için
  ! platformlar arası geliştirmeyi destekleyecek şekilde
  * yazılımdaki nesneleri
  * ilişkisel bir veritabanının tablo ve sütunlarına eşlemeyi basitleştiren
  * bir Nesne İlişkisel Eşleyici bir veri erişim teknolojisi
  * olarak açık kaynaklı bir şekilde piyasaya sürüldü.

  ? dotnet tool install --global dotnet-ef

*/


/**
  * * Bir ORM aracının hedef veritabanını OOP imkanlarıyla temsil edebilmesi için veritabanının, o veritabanı içerisindeki tabloların ve o tablolar içerisindeki
kolon ve nesnelerin kod tarafında programatik olarak modellenmesi gerekmektedir.

  * * Bu modelleme EF Core dünyasında DBContext, Entity ve Property gibi birtakım aktörler aracılığıyla gerçekleşir.      (02)
  * * Ayrıca EF Core bu modellemeyi Database First ve Code First olmak üzere iki farklı yaklaşımla gerçekleştirebilir.    (03)

      1. Database First
      2. Code First 
*/

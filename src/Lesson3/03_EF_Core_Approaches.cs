/* Entity Framework Core Approaches */

/** Database First

  * * Database First yaklaşımı hedef veritabanının belirli talimatlar aracılığıyla(scaffold) otomatik olarak kod kısmına OOP imkanları eşliğinde modellenmesidir.
  * * Bu modelleme kabaca hedef veritabanını temsil eden bir DbContext sınıfı, veritabanındaki tabloları temsil eden birçok Class ve tabloların içindeki sütunları temsil eden birçok Property oluşturur.
  * * EF Core ile çalışma yapılacak olan veritabanı önceden oluşturulmuş ise Database First yaklaşımı tercih edilmelidir.

*/


/** Code First

  * * Code First yaklaşımı veritabanını kod tarafında modelleyerek ardından bu modele uygun veritabanını sunucuda oluşturan yaklaşımdır. 
  * * Bu modelleme kabaca hedef veritabanını temsil edecek olan bir DbContext sınıfını, veritabanındaki tabloları temsil edecek olan Class'ları ve tabloların içindeki sütunları temsil edecek Property'leri kod tarafında manuel olarak yazmayı gerektirir.
  * * Dolayısıyla veri tabanının kod üzerinden modellenebilmesini sağlar.
*/
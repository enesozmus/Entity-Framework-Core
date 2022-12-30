using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Inheritance
/**

    * * Entity'lerin birbirlerinden kalıtım alma durumlarıdır.
    * * EF Core can map a .NET type hierarchy to a database.
    * * This allows you to write your .NET entities in code as usual, using base and derived types, and have EF Core  seamlessly create the appropriate database schema, issue queries, etc.
    * * By convention, EF Core will not automatically scan for base or derived types; this means that if you want a CLR type in your hierarchy to be mapped, you must explicitly specify that type on your model.

*/
#endregion

#region Table-per-hierarchy
/**

    * * EF Core will represent an object-oriented hierarchy in a single table that takes the name of the base class and includes a "discriminator" column to identify the specific type for each row.
    * * TPH uses a single table to store the data for all types in the hierarchy, and a discriminator column is used to identify which type each row represents.
    * * By default, EF Core maps the inheritance using the table-per-hierarchy (TPH) pattern.

    * * Bu davranışta hiyerarşi başına, hiyerarşi içindeki tüm entity’ler için, tek bir tablo oluşturulmakta ve bu tabloda veriler tutulurken base class’dan gelen alanlar doldurulmakta, olmayan alanlar için nullable şartı gelmektedir.
    * * Bir başka deyişle, veritabanındaki bir tablo EF Core tarafında birden fazla entity’nin bütünsel haline karşılık gelmektedir.
    * * Bir başka deyişle, kalıtımsal hiyerarşideki tüm entity'ler için tek bir tablo oluşturulmakta ve bunlar arasındaki ayrım için bir DISCRIMINATOR property'si oluşturulmaktadır.

    * * Bu EF Core'da entity'ler aransında temel bir kalıtımsal ilişki söz konusuysa default oalrak kabul edilen davranıştır.
    * * Dolayısıyla ayrıca herhangi bir konfigüreasyon gerektirmez!
    * * Entityler kendi aralarında kalıtımsal ilişkiye sahip olmalı ve bu entity'lerin hepsi DbContext sınıfına DbSet<T> olarak eklenmelidir!

*/
#region Discriminator Column
// Table-per-hierarchy yaklaşımı neticesinde kümülatif olarak inşa edilmiş tablonun hangi entity'e karşılık veri tuttuğunu ayırt edebilmemizi sağlayan bir kolondur.
// EF Core tarafından otomatik olarak tabloya yerleştirilir.
// Default olarak içerisinde entity isimlerini tutar.
// Discriminator kolonu HasDiscriminator() yöntemi ile özelleştirilebilir.
#endregion
#endregion

#region Table-per-type
/**

    * * In the TPT mapping pattern, all the types are mapped to individual tables. 
    * * Properties that belong solely to a base type or derived type are stored in a table that maps to that type.
    * * Tables that map to derived types also store a foreign key that joins the derived table with the base table.

    * * Veritabanı açısından bir tablodaki belirli kolonların bağımsız olarak birebir ilişki ile farklı tablolarda tutulmasıdır.
    * * Bir başka deyişle, entity'lerin aralarında kalıtımsal ilişkiye sahip olduğu durumlarda her bir türe/entitye/tip/referans karşılık bir tablo generate eden davranıştır.
    * * Bir başka deyişle, kalıtımsal hiyerarşideki her entitiy için ayrı bir tablo oluşturulmakta ve kalıtımsal olarak bir üst sınıfla bire-bir ilişki kurulmaktadır.

*/
#endregion

#region Table-per-concrete-type
/**

    * * The table-per-concrete-type (TPC) feature was introduced in EF Core 7.0.
    * * In the TPC mapping pattern, all the types are mapped to individual tables.
    * * Each table contains columns for all properties on the corresponding entity type.
    * * This addresses some common performance issues with the TPT strategy.

    * * Her somut tür için bir tablo...
    * * Kalıtımsal hiyerarşideki sadece 'concrete' sınıflara karşılık birer tablo oluşturmakta ve abstract yapılanmaların member'larını bu tablolara eklemektedir.

*/
#endregion
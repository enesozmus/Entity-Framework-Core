Console.WriteLine("Hello, World!");

#region EF Core and SQL Sequences
/**

    * * A sequence generates unique, sequential numeric values in the database.
    * * Sequences are not associated with a specific table, and multiple tables can be set up to draw values from the same sequence.

    * * You can set up a sequence in the model, and then use it to generate values for properties:

        - modelBuilder.HasSequence("EC_Sequence").StartsAt(100).IncrementsBy(5);
        - modelBuilder.Entity<Employee>().Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR EC_Sequence");
        - modelBuilder.HasSequence<int>("OrderNumbers");
        - modelBuilder.Entity<Order>().Property(o => o.OrderNo).HasDefaultValueSql("NEXT VALUE FOR OrderNumbers");

    * * You can also configure additional aspects of the sequence, such as its schema, start value, increment, etc.:

        - modelBuilder.HasSequence<int>("OrderNumbers", schema: "shared").StartsAt(1000).IncrementsBy(5);

    * ! Sequence'ler üzerinden değer oluştururken veritabanına özgü çalışma yapılması zaruridir. SQL Server'a özel yazılan Sequence tanımı misal olarak Oracle için hata verebilir.

*/
#endregion

#region Sequence İle Identity Farkı
// Sequence bir veritabanı nesnesiyken, Identity ise tabloların özellikleridir.
// Yani Sequence herhangi bir tabloya bağımlı değildir. 
// Identity bir sonraki değeri diskten alırken Sequence ise RAM'den alır.
// Bu yüzden önemli ölçüde Identity'e nazaran daha hızlı, performanslı ve az maliyetlidir.
#endregion
Console.WriteLine("Hello, World!");

#region EF Core and SQL Indexes
/**

    * * Indexes are special lookup tables that the database search engine can use to speed up data retrieval.
    * * Indexes can be created or dropped with no effect on the data.
    * * The users cannot see the indexes, they are just used to speed up searches/queries.
    * * An index helps to speed up SELECT queries and WHERE clauses, but it slows down data input, with the UPDATE and the INSERT statements.
    * * So, only create indexes on columns that will be frequently searched against.

    * * Index, bir sütuna dayalı sorgulamaları daha verimli ve performanslı hale getirmek için kullanılan yapıdır.
    * * Index yapılandırmaları EF Core üzerinden de yapılabilir.
    * * PK, FK ve AK olan kolonlar otomatik olarak indexlenir.
    
    * * You can specify an index over a column as follows:

        - [Index(nameof(Url))]
        - [Index(nameof(Name), nameof(Surname))]
        - [Index(nameof(Name), AllDescending = true)]
        - [Index(nameof(Name), nameof(Surname), IsDescending = new[] { true, false })]
        - [Index(nameof(Name), Name = "name_index")]
        - modelBuilder.Entity<Blog>().HasIndex(b => b.Url);
        - .HasIndex(x => x.Name);
        - .HasIndex(x => new { x.Name, x.Surname });
        - .HasIndex(nameof(Employee.Name), nameof(Employee.Surname));

    * * An index can also span more than one column:

        - [Index(nameof(FirstName), nameof(LastName))]
        - modelBuilder.Entity<Person>().HasIndex(p => new { p.FirstName, p.LastName });

    * * By default, indexes aren't unique: multiple rows are allowed to have the same value(s) for the index's column set.
    * * You can make an index unique as follows:

        - [Index(nameof(Url), IsUnique = true)]
        - modelBuilder.Entity<Blog>().HasIndex(b => b.Url).IsUnique();


    Yorum: index select zamanı performans kazandırıyorsa, create zamanı ise performansdan veriyor. çünki index olduğu column-a göre yeniden sıralıyor.
    o yüzden büyük projelerde CQRS pattern uygulanıyor. standby veri tabanları oluyor.sorgular selectler ayrı veri tabanından sorgulanıyor ama
    bir veri eklendiği zaman ise diğer bir veri tabanına kayıt ediliyor. bu iki veri tabanı bir birine sync olarak çalışıyor.

*/
#endregion

#region Index sort order
/**

    * * This feature is being introduced in EF Core 7.0.
    * * In most databases, each column covered by an index can be either ascending or descending.
    * * For indexes covering only one column, this typically does not matter: the database can traverse the index in reverse order as needed.
    * * However, for composite indexes, the ordering can be crucial for good performance, and can mean the difference between an index getting used by a query or not.
    * * In general, the index columns' sort orders should correspond to those specified in the ORDER BY clause of your query.
    * * The index sort order is ascending by default. You can make all columns have descending order as follows:

        - [Index(nameof(Url), nameof(Rating), AllDescending = true)]
        - modelBuilder.Entity<Blog>().HasIndex(b => new { b.Url, b.Rating }).IsDescending();
        - .IsDescending();
        - .IsDescending(true, false);

    * * You may also specify the sort order on a column-by-column basis as follows:

        - [Index(nameof(Url), nameof(Rating), IsDescending = new[] { false, true })]
        - modelBuilder.Entity<Blog>().HasIndex(b => new { b.Url, b.Rating }).IsDescending(false, true);

    * * By convention, indexes created in a relational database are named IX_<type name>_<property name>.

        - [Index(nameof(Url), Name = "Index_Url")]
        - modelBuilder.Entity<Blog>().HasIndex(b => b.Url).HasDatabaseName("Index_Url");

*/
#endregion

#region Index filter
/**
    * * Some relational databases allow you to specify a filtered or partial index.
    * * This allows you to index only a subset of a column's values, reducing the index's size and improving both performance and disk space usage.
    * * You can use the Fluent API to specify a filter on an index, provided as a SQL expression:
        - modelBuilder.Entity<Blog>().HasIndex(b => b.Url).HasFilter("[Url] IS NOT NULL");

    * * When using the SQL Server provider EF adds an 'IS NOT NULL' filter for all nullable columns that are part of a unique index. To override this convention you can supply a null value.
        - modelBuilder.Entity<Blog>().HasIndex(b => b.Url).IsUnique().HasFilter(null);
        
*/
#endregion


#region Included columns
// Some relational databases allow you to configure a set of columns which get included in the index, but aren't part of its "key". 
// This can significantly improve query performance when all columns in the query are included in the index either as key or nonkey columns, as the table itself doesn't need to be accessed.
// In the following example, the Url column is part of the index key, so any query filtering on that column can use the index.
// But in addition, queries accessing only the Title and PublishedOn columns will not need to access the table and will run more efficiently:
// modelBuilder.Entity<Post>().HasIndex(p => p.Url).IncludeProperties(p => new { p.Title, p.PublishedOn });
#endregion
#region Check constraints
// Check constraints are a standard relational feature that allows you to define a condition that must hold for all rows in a table; any attempt to insert or modify data that violates the constraint will fail.
// Check constraints are similar to non-null constraints (which prohibit nulls in a column) or to unique constraints (which prohibit duplicates), but allow arbitrary SQL expression to be defined.
// Multiple check constraints can be defined on the same table, each with their own name.
// Some common check constraints can be configured via the community package EFCore.CheckConstraints.
// You can use the Fluent API to specify a check constraint on a table, provided as a SQL expression:
// modelBuilder.Entity<Product>().ToTable(b => b.HasCheckConstraint("CK_Prices", "[Price] > [DiscountedPrice]"));
#endregion
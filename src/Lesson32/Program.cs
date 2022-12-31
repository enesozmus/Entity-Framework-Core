Console.WriteLine("Hello, World!");

#region Key
/**

  * * A key serves as a unique identifier for each entity instance.
  * * Most entities in EF have a single key, which maps to the concept of a primary key in relational databases.
  * * Entities can have additional keys beyond the primary key.

*/
#endregion

#region Keyless Entity Types
/**

    * * In addition to regular entity types, an EF Core model can contain keyless entity types, which can be used to carry out database queries against data that doesn't contain key values.

    - [Keyless]
    - modelBuilder.Entity<BlogPostsCount>().HasNoKey();

*/
#endregion


#region Configuring a primary key
/**

    * * Principal key: The properties that uniquely identify the principal entity. This may be the primary key or an alternate key.
    * * Foreign key: The properties in the dependent entity that are used to store the principal key values for the related entity.
    * * By convention, a property named Id or <type name>Id will be configured as the primary key of an entity.

    - public string Id { get; set; }
    - public string TruckId { get; set; }

    * * You can configure a single property to be the primary key of an entity as follows:

    - [Key]
    - modelBuilder.Entity<Car>().HasKey(c => c.LicensePlate);

    * * Entity Framework Core supports composite keys - primary key values generated from two or more fields in the database.
    * * You can also configure multiple properties to be the key of an entity - this is known as a composite key.

      - modelBuilder.Entity<Car>().HasKey(c => new { c.State, c.LicensePlate });
      - X [PrimaryKey(nameof(State), nameof(LicensePlate))]

*/
#endregion

#region SQL Constraints
/**

    * * SQL Constraint (kısıtlamalar), bir tablodaki veriler için kurallar belirtmek için kullandığımız yapıdır.
    * * Bir tabloya girebilecek veri türünü sınırlamak için Constraint (kısıtlamalar) kullanılır. Kısıtlama ile veri eylemi arasında herhangi bir ihlal varsa, eylem iptal edilir.
    * * Bu, tablodaki verilerin doğruluğunu ve güvenilirliğini sağlar.

        - NOT NULL      – Bir sütunun NULL değerine sahip olmamasını sağlar.
                        - IsRequired()
                         → Configures whether this property must have a value assigned or whether null is a valid value.

        - UNIQUE        – Bir sütundaki tüm değerlerin farklı olmasını sağlar.
                        - IsUnique()   
                        → Configures the index to be unique.

        - PRIMARY KEY   – NOT NULL ve UNIQUE kombinasyonu. Bir tablodaki her satırı benzersiz bir şekilde tanımlar.
                        - HasKey()     
                        → Sets the properties that make up the primary key for this entity type.

        - FOREIGN KEY   – Tablolar arasındaki bağlantıları yok edecek eylemleri önler.
                        - HasForeignKey()   
                        → Configures the property(s) to use as the foreign key for this relationship.

        - CHECK         – Bir sütundaki değerlerin belirli bir koşulu karşılamasını sağlar.
                        - HasCheckConstraint()
                        →

        - DEFAULT       – Değer belirtilmemişse bir sütun için varsayılan bir değer ayarlar.
                        -
                        →

        - CREATE-INDEX  – Veri tabanından çok hızlı bir şekilde veri oluşturmak ve almak için kullanılır.
                        -
                        →

*/
#endregion

#region Alternate Keys
/**

    * * An alternate key serves as an alternate unique identifier for each entity instance in addition to the primary key; it can be used as the target of a relationship.
    * * When using a relational database this maps to the concept of a unique index/constraint on the alternate key column(s) and one or more foreign key constraints that reference the column(s).
    * * If you just want to enforce uniqueness on a column, define a unique index rather than an alternate key (see Indexes).
    * * EF, alternate keys are read-only and provide additional semantics over unique indexes because they can be used as the target of a foreign key.

    - .HasAlternateKey(c => c.LicensePlate);
    - .HasAlternateKey(c => new { c.State, c.LicensePlate });

*/
#endregion
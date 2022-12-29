Console.WriteLine("Hello, World!");

#region Customizing Entity Configurations 2 | Just Fluent API

#region Composite Key
// Tablolarda birden fazla property'nin kümülatif olarak Primary Key olarak yapılandırıldığı durumdur.
#endregion

#region HasDefaultSchema
// EF Core üzerinden inşa edilen herhangi bir veritabanı nesnesi default olarak dbo şemasına sahiptir.
// Bunu özelleştirebilmek için kullanılan bir yapılandırmadır.
#endregion

#region HasDefaultValue
// Tablodaki herhangi bir kolonun değer gönderilmediği durumlarda default olarak hangi değeri alacağını belirler.
#endregion

#region HasDefaultValueSql
// Tablodaki herhangi bir kolonun değer gönderilmediği durumlarda default olarak hangi değeri alacağını sql ifadesi ile belirler.
#endregion

#region HasComputedColumnSql
// Tablolarda birden fazla kolondaki verileri işleyerek değerini oluşturan kolonlara Computed Column denmektedir.
// EF Core üzerinden bu tarz computed column oluşturabilmek için kullanılan bir yapılandırmadır.
#endregion

#region HasConstraintName
// EF Core üzerinden oluşturulan constraint'lere default isim yerine özelleştirilmiş bir isim verebilmek için kullanılan yapılandırmadır.
#endregion

#region HasData
// EF Core Fluent API HasData() yöntemi, belirtilen entity'ye migration'lar için seed/tohum verileri sağlamaya yardımcı olmak için tasarlanmıştır.
// Bu, bir veritabanının bilinen bir durumda başlaması gerektiğinde test amaçları için yararlı olabilir.
// Key değeri gereklidir, bu nedenle sağlanmalıdır.
// HasData yöntemi kullanıldığında, EF Core ilgili tablo için otomatik olarak SET IDENTITY INSERT ON oluşturacak ve tohumlama tamamlandıktan sonra bunu OFF olarak ayarlayacaktır.
#endregion

#region HasField
// Backing Field özelliğini kullanmamızı sağlayan bir yapılandırmadır.
#endregion

#region HasNoKey
// Normal şartlarda EF Core'da tüm entity'lerin bir Primary Key kolonu olmak zorundadır.
// Tasarladığımız entity'de Primary Key kolonu olmayacaksa olmasın istiyorsak bunun bildirilmesi gerekmektedir!
// İşte bunun için kullanılan yapılandırmadır.
#endregion

#region HasIndex
// ...sonraki dersler...
#endregion

#region HasQueryFilter
// ...sonraki dersler...
#endregion

#region HasDiscriminator
// ...sonraki dersler...
#endregion
#endregion

#region Composite Key
// modelBuilder.Entity<Person>().HasKey("Id", "Id2");
// modelBuilder.Entity<Person>().HasKey(p => new { p.Id, p.Id2 });
#endregion
#region HasDefaultSchema
// modelBuilder.HasDefaultSchema(".....");
#endregion
#region HasDefaultValue
//modelBuilder.Entity<Person>()
// .Property(p => p.Salary)
// .HasDefaultValue(100);
#endregion
#region HasDefaultValueSql
//modelBuilder.Entity<Person>()
//    .Property(p => p.CreatedDate)
//    .HasDefaultValueSql("GETDATE()");
#endregion
#region HasComputedColumnSql
//modelBuilder.Entity<Example>()
//    .Property(p => p.Computed)
//    .HasComputedColumnSql("[X] + [Y]");
#endregion
#region HasConstraintName
//modelBuilder.Entity<Person>()
//    .HasOne(p => p.Department)
//    .WithMany(d => d.Persons)
//    .HasForeignKey(p => p.DepartmentId)
//    .HasConstraintName("..........");
#endregion
#region HasData
//modelBuilder.Entity<Department>().HasData(
//    new Department()
//    {
//        Name = "asd",
//        Id = 1
//    });
//modelBuilder.Entity<Person>().HasData(
//    new Person
//    {
//        Id = 1,
//        DepartmentId = 1,
//        Name = "ahmet",
//        Surname = "filanca",
//        Salary = 100,
//        CreatedDate = DateTime.Now
//    },
//    new Person
//    {
//        Id = 2,
//        DepartmentId = 1,
//        Name = "mehmet",
//        Surname = "filanca",
//        Salary = 200,
//        CreatedDate = DateTime.Now
//    }
//    );
#endregion
#region HasField
//modelBuilder.Entity<Person>()
//    .Property(p => p.Name)
//    .HasField(nameof(Person._name));
#endregion
#region HasNoKey
//modelBuilder.Entity<Example>()
//    .HasNoKey();
#endregion
#region HasIndex
//modelBuilder.Entity<Person>()
//    .HasIndex(p => new { p.Name, p.Surname });
#endregion
#region HasQueryFilter
//modelBuilder.Entity<Person>()
//    .HasQueryFilter(p => p.CreatedDate.Year == DateTime.Now.Year);
#endregion
#region HasDiscriminator
//modelBuilder.Entity<Entity>()
//    .HasDiscriminator<int>("Ayirici")
//    .HasValue<A>(1)
//    .HasValue<B>(2)
//    .HasValue<Entity>(3);
#endregion
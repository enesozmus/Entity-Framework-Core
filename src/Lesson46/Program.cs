Console.WriteLine("Hello, World!");

#region Owned Entity Types
/**

  * ! EF Core allows you to model entity types that can only ever appear on navigation properties of other entity types.
  * ! These are called owned entity types. The entity containing an owned entity type is its owner.
  * ! Owned entities are essentially a part of the owner and cannot exist without it, they are conceptually similar to aggregates.
  * ! This means that the owned entity is by definition on the dependent side of the relationship with the owner.
  
  * * EF Core tasarımlarında bir entity’nin olması gereken inşa kurallarının dışına çıkarak parçalamak ve grupsal olarak property’lerini farklı class’lar da tutmak isteyebiliriz.
  * * Bu class’lar, yapısal olarak entity type’a benzeyebilirler lakin davranışsal olarak, ana entity’nin farklı property’lerini temsil etmektedirler.
  * * İşte bir entity'nin bazı property'lerini gruplayarak farklı bir sınıfta tutabilmemizi sağlayabilen bir Entity Type'ıdır.
  * * Böylece bir entity, sahip olunan(owned) birden fazla alt sınıfın birleşmesiyle meydana gelebilmektedir.
  * ? Domain Driven Design(DDD) yaklaşımında Value Object'lere karşılık olarak Owned Entity Types'lar sıkça kullanılırlar! 
*/
#endregion


#region Configuring types as owned
/*

  * * In most providers, entity types are never configured as owned by convention.
  * * You must explicitly use the OwnsOne method in OnModelCreating or annotate the type with OwnedAttribute to configure the type as owned.

  * * In this example, StreetAddress is a type with no identity property.
  * * It is used as a property of the Order type to specify the shipping address for a particular order.
  * * We can use the OwnedAttribute to treat it as an owned entity when referenced from another entity type:

    [Owned]
    public class StreetAddress
    {
      public string Street { get; set; }
      public string City { get; set; }
    }
    public class Order
    {
      public int Id { get; set; }
      public StreetAddress ShippingAddress { get; set; }
    }

  * ! It is also possible to use the OwnsOne method in OnModelCreating to specify that the ShippingAddress property is an Owned Entity of the Order entity type and to configure additional facets if needed.

    - modelBuilder.Entity<Order>().OwnsOne(p => p.ShippingAddress);

  * ! If the ShippingAddress property is private in the Order type, you can use the string version of the OwnsOne method:

    - modelBuilder.Entity<Order>().OwnsOne(typeof(StreetAddress), "ShippingAddress");
*/
#endregion


#region [Owned] and OwnsOne()
/**

  * * Normal şartlarda bir entity'de farklı sınıfların referans edilmesi EF Core tarafından ilişkisel bir tasarım olarak algılanacaktır.
  * * Dolayısıyla entity içerisindeki property'leri kümesel olarak barındıran sınıfları o entity'nin bir parçası olduğunu bildirmemiz gerekmektedir.

      - [Owned]

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        #region OwnsOne()
          modelBuilder.Entity<Employee>().OwnsOne(e => e.EmployeeName, builder =>
          {
            // isme müdahale
            builder.Property(e => e.Name).HasColumnName("Name");
          });
          modelBuilder.Entity<Employee>().OwnsOne(e => e.Adress);
        #endregion

        #region OwnsMany()
        // OwnsMany() metodu, entity'nin farklı özelliklerine başka bir sınıftan ICollection türünde Navigation Property aracılığıyla ilişkisel olarak erişebilmemizi sağlayan bir işleve sahiptir.
        modelBuilder.Entity<Employee>().OwnsMany(e => e.Orders, builder =>
        {
            builder.WithOwner().HasForeignKey("OwnedEmployeeId");
            builder.Property<int>("Id");
            builder.HasKey("Id");
        });
        #endregion
      }
*/
#endregion


#region Example - 1
/*

class Employee
{
    public int Id { get; set; }
    //public string Name { get; set; }
    //public string MiddleName { get; set; }
    //public string LastName { get; set; }
    //public string StreetAddress { get; set; }
    //public string Location { get; set; }
    public bool IsActive { get; set; }

    public EmployeeName EmployeeName { get; set; }        // §-§ → [Owned]
    public Address Adress { get; set; }                   // §-§ → [Owned]

    public ICollection<Order> Orders { get; set; }
}
class Order
{
    public string OrderDate { get; set; }
    public int Price { get; set; }
}
[Owned]
class EmployeeName
{
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public EmployeBilmemneName EmployeBilmemneName { get; set; }    // §-§ → [Owned]
}
[Owned]
class Address
{
    public string StreetAddress { get; set; }
    public string Location { get; set; }
}
*/
#endregion


#region IEntityTypeConfiguration<T> Interface
// class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
// {
//   public void Configure(EntityTypeBuilder<Employee> builder)
//   {
//     builder.OwnsOne(e => e.EmployeeName, builder =>
//     {
//       builder.Property(e => e.Name).HasColumnName("Name");
//     });
//     builder.OwnsOne(e => e.Adress);
//   }
// }
#endregion
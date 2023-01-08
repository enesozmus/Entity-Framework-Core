#region Entity splitting
/*

  ? https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#entity-splitting

  ½ Birden fazla fiziksel tabloyu Entity Framework Core kısmında tek bir entity ile temsil etmemizi sağlayan bir özelliktir.

  * Entity splitting maps a single entity type to multiple tables.
  * For example, consider a database with three tables that hold customer data:

        1. A Customers table for customer information
        2. A PhoneNumbers table for the customer's phone number
        3. A Addresses table for the customer's address
  
  * If all three tables are always used together, then it can be more convenient to map them all to a single entity type.

  ! This is achieved in EF7 by calling SplitToTable for each split in the entity type.
  ! For example, the following code splits the Customer entity type to the Customers, PhoneNumbers, and Addresses tables shown above:
  + Notice also that, if necessary, different primary key column names can be specified for each of the tables.

  modelBuilder.Entity<Customer>(entityBuilder =>
  {
      entityBuilder.ToTable("Customers")
          .SplitToTable("PhoneNumbers", tableBuilder =>
          {
              tableBuilder.Property(customer => customer.Id).HasColumnName("CustomerId");
              tableBuilder.Property(customer => customer.PhoneNumber);
          })
          .SplitToTable("Addresses", tableBuilder =>
          {
              tableBuilder.Property(customer => customer.Id).HasColumnName("CustomerId");
              tableBuilder.Property(customer => customer.Street);
              tableBuilder.Property(customer => customer.City);
              tableBuilder.Property(customer => customer.PostCode);
              tableBuilder.Property(customer => customer.Country);
          });
  });
*/
public class Customer
{
  public Customer(string name, string street, string city, string? postCode, string country)
  {
    Name = name;
    Street = street;
    City = city;
    PostCode = postCode;
    Country = country;
  }

  public int Id { get; set; }
  public string Name { get; set; }
  public string? PhoneNumber { get; set; }
  public string Street { get; set; }
  public string City { get; set; }
  public string? PostCode { get; set; }
  public string Country { get; set; }
}
#endregion
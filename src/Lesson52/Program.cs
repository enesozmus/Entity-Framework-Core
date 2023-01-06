Console.WriteLine("Hello, World!");

#region Value Conversions in Entity Framework Core
/*

  * Value converters allow property values to be converted when reading from or writing to the database.
  * This conversion can be from one value to another of the same type or from a value of one type to a value of another type.

  * Value converters are specified in terms of a ModelClrType and a ProviderClrType.
  * The model type is the .NET type of the property in the entity type.
  * The provider type is the .NET type understood by the database provider.

  * Conversions are defined using two Func expression trees: one from ModelClrType to ProviderClrType and the other from ProviderClrType to ModelClrType.
  * Expression trees are used so that they can be compiled into the database access delegate for efficient conversions.
  * The expression tree may contain a simple call to a conversion method for complex conversions.

  + EF Core, verisel açıdan belli başlı değerlere olan bağımlılıkları ortadan kaldırabilme niteliğiyle eşsiz bir hizmet sağlayabiliyor.
  + EF Core ile yapılan sorgulamalarda salt bir şekilde sorgu çekilirse eğer veriler ne ise o şekilde elde edilecektir.
  + Fakat Value Converters özelliği sayesinde gelecek olan veriler, veritabanından sorgulandıktan sonra farklı değerlere dönüştürülerek orjinalleriyle ilişkili olabilecek şekilde elde edilebilir.
  + https://www.gencayyildiz.com/blog/entity-framework-core-value-converters/
*/
#endregion


#region HasConversion()
/*

  * Value conversions are configured in DbContext.OnModelCreating.
  * HasConversion() fonksiyonu, en sade haliyle EF Core üzerinden Value Converter özelliği gören bir fonksiyondur.

  public class Person
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public bool Married { get; set; }
  }

    - modelBuilder.Entity<Person>()
                      .Property(p => p.Gender)
                      ↓(INSERT - UPDATE, SELECT)↓
                      .HasConversion(g => g.ToUpper(), g => g == "M" ? "Male" : "Female");

    - var persons = await _context.Persons.ToListAsync();
*/
#endregion


#region Working with enums
/*

  * For example, consider an enum and entity type defined as:

  + Normal durumlarda enum türünde tutulan property'lerin veritabanındaki karşılıkları int olacak şekilde aktarımı gerçekleştirlimektedir. 
  + Value converter sayesinde enum türünden olan property'lerin de dönüşümlerini istediğimiz türlere sağlayarak hem ilgili kolonun türünü o türde ayarlayabilir
  + ...hem de enum üzerinden çalışma sürecinde verisel dönüşümleri ilgili türde sağlayabiliriz.

    public class Rider
    {
      public int Id { get; set; }
      public EquineBeast Mount { get; set; }
    }

    public enum EquineBeast
    {
      Donkey,
      Mule,
      Horse,
      Unicorn
    }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
        .Entity<Rider>()
        .Property(e => e.Mount)
        .HasConversion(
            v => v.ToString(),
            v => (EquineBeast)Enum.Parse(typeof(EquineBeast), v));
  }

  - var person = new Person() { Name = "David", Gender2 = Gender.Male, Gender = "M" };
  - await _context.Persons.AddAsync(person);
  - await _context.SaveChangesAsync();
  - var _person = await _context.Persons.FindAsync(person.Id);
*/
#endregion


#region abstract class ValueConverter : ValueConverter
/*

  * Defines conversions from an object of one type in a model to an object of the same or different type in the store.
  * Initializes a new instance of the Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter`2 class.

      1. convertToProviderExpression: An expression to convert objects when writing data to the store.
      2. convertFromProviderExpression: An expression to convert objects when reading data from the store.
  * Defines conversions from an object of one type in a model to an object of the same or different type in the store.
  + ValueConverter class'ı, verisel dönüşümlerdeki çalışmaları/sorumlulukları üstlenebilecek bir sınıftır.
  + Yani bu class'ın instance'ı ile HasConvention() fonksiyonu içerisinde yapılan çalışmaları üstlenebilir ve
  + ...direkt bu instance'ı ilgili fonksiyona vererek dönüşümsel çalışmalarımızı gerçekleştirebiliiriz.

      ValueConverter<Gender, string> converter = new(
          //INSERT - UPDATE
          g => g.ToString()
          ,
          //SELECT
          g => (Gender)Enum.Parse(typeof(Gender), g)
        );

      modelBuilder.Entity<Person>()
                    .Property(p => p.Gender2)
                    .HasConversion(converter);
*/
#endregion


#region Custom ValueConverter Class
/*

  * Defines conversions from an object of one type in a model to an object of the same or different type in the store.
  + EF Core'da verisel dönüşümler için custom olarak converter sınıfları üretebilmekteyiz.
  + Bunun için tek yapılması gereken custom sınıfın ValueConverter abstract class'ından miras almasını sağlamaktadır.

    modelBuilder.Entity<Person>()
          .Property(p => p.Gender2)
          .HasConversion<GenderConverter>();

    public class GenderConverter : ValueConverter<Gender, string>
    {
        public GenderConverter() : base(
            //INSERT - UPDATE
            g => g.ToString()
            ,
            //SELECT
            g => (Gender)Enum.Parse(typeof(Gender), g)
            )
        {
        }
    }
*/
#endregion


#region Built-in converters
/*

  + EF Core basit dönüşümler için kendi bünyesinde yerleşik convert sınıfları barındırmaktadır.
  * EF Core ships with a set of pre-defined ValueConverter<TModel,TProvider> classes, found in the Microsoft.EntityFrameworkCore.Storage.ValueConversion namespace.
  * In many cases EF will choose the appropriate built-in converter based on the type of the property in the model and the type requested in the database, as shown above for enums.
  * For example, using .HasConversion<int>() on a bool property will cause EF Core to convert bool values to numerical zero and one values:

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder
          .Entity<User>()
          .Property(e => e.IsActive)
          .HasConversion<int>();
    }

  * This is functionally the same as creating an instance of the built-in BoolToZeroOneConverter<TProvider> and setting it explicitly:

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
      var converter = new BoolToZeroOneConverter<int>();

      modelBuilder
          .Entity<User>()
          .Property(e => e.IsActive)
          .HasConversion(converter);
  }

  ? BoolToZeroOneConverter(): bool olan verinin int olarak tutulmasını sağlar.
  ? BoolToStringConverter(): bool olan verinin string olarak tutulmasını sağlar.
  ? BoolToTwoValuesConverter(): bool olan verinin char olarak tutulmasını sağlar.
  ! For more: https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations
*/
#endregion


#region Collections of primitives
/*

  * Serialization can also be used to store a collection of primitive values. For example:
  + İçerisinde ilkel türlerden oluşturulmuş koleksiyonları barındıran modelleri migrate etmeye çalıştığımızda hata ile karşılaşmaktayız.
  + Bu hatadan kurtuılmak ve ilgili veriye koleksiyondaki verileri serilize ederek işleyebilmek için bu koleksiyonu normal metinsel değerlere
  + ...dönüştürmemize fırsat veren bir conversion operasyonu gerçekleştireibliriz.

  public class Post
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Contents { get; set; }

    * ICollection<string> represents a mutable reference type.
    *  This means that a ValueComparer<T> is needed so that EF Core can track and detect changes correctly.
    public ICollection<string> Tags { get; set; }
  }

  modelBuilder.Entity<Post>()
    .Property(e => e.Tags)
    .HasConversion(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null),
        new ValueComparer<ICollection<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => (ICollection<string>)c.ToList()
        )
    );
*/
#endregion


#region .NET 6 - Value Converter For Nullable Fields
// .NET 6'dan önce value converter'lar null değerlerin dönüşümünü desteklememekteydi.
// .NET 6 ile artık null ldeğerler de dönüştürülebilmektedir.
#endregion
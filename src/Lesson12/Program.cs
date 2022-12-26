// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region Other Querying Methods
#region Count()
/**

  * * Returns an Int32 that represents the number of elements in a sequence.
  * * Returns the number of elements in the specified sequence that satisfies a condition.
  * * Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(int) bizlere bildiren fonksiyondur.

      - int numberOfFruits = fruits.AsQueryable().Count();
      - int numberUnvaccinated = pets.AsQueryable().Count(p => p.Vaccinated == false);
      - X int number_of_students = (await _context.Students.ToListAsync()).Count();
      - ✔ int number_of_students = await _context.Students.CountAsync();

      + Console.WriteLine("There are {0} items in the array.", numberOfFruits);
      + Console.WriteLine(number_of_students);

*/
#endregion


#region LongCount()
/**

  * * Returns an Int64 that represents the number of elements in a sequence.
  * * Returns an Int64 that represents the number of elements in a sequence that satisfy a condition.
  * * Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(long) bizlere bildiren fonksiyondur.

      - long count = fruits.AsQueryable().LongCount();
      - long count = pets.AsQueryable().LongCount(pet => pet.Age > Age);
      - ✔ long number_of_students = await _context.Students.LongCountAsync();

      + Console.WriteLine("There are {0} fruits in the collection.", count);
      + Console.WriteLine("There are {0} animals over age {1}.", count, Age);
      + Console.WriteLine(number_of_students);

*/
#endregion


#region Any()
/**

  * * Determines whether a sequence contains any elements.
  * * Determines whether any element of an IQueryable<T> sequence exists or satisfies a condition.
  * * returns true if the source sequence contains any elements; otherwise, false.
  * * Sorgu neticesinde verinin gelip gelmediğini bool türünde dönen fonksiyondur.

      - X bool hasElements = numbers.Any();
      - ✔ bool hasElements = numbers.AsQueryable().Any();
      - bool isExist = _context.Students.AsQueryable().Any();
      - bool isExist = _context.Students.AsQueryable().Any(x => x.FirstName == "Ziya");
      - bool isExist = await _context.Students.Where(x => x.FirstName.Contains("B")).AnyAsync();

*/
#endregion


#region Max() & Min()
/**

  * * determines the maximum/minimum value in a sequence of projected values.
  * * returns the maximum/minimum in the sequence.
  * * Verilen kolondaki max/min değeri getirir.

      // ? List<long> longs = new List<long> { 4294967296L, 466855135L, 81125L };
      // ? long max = longs.AsQueryable().Max();
      // ? double min = doubles.AsQueryable().Min();

      - int maxID = await _context.Students.MaxAsync(x => x.ID);
      - int maxID = await _context.Students.AsQueryable().MaxAsync(x => x.ID);

*/
#endregion


#region Distinct()
/**

  * * Returns distinct elements from a sequence by using a specified IEqualityComparer<T> to compare values.
  * * Sorguda mükerrer kayıtlar varsa bunları tekilleştiren bir işleve sahip fonksiyondur.
  * * Distinct bir alan üzerinde tekrarlayan (benzer) kayıtları bir kere görmek istenildiği zaman kullanılmaktadır.

      // ? List<int> ages = new List<int> { 21, 21, 21, 46, 46, 55, 17, 21, 55, 55 };
      // ? IEnumerable<int> distinctAges = ages.AsQueryable().Distinct();
      // ? Console.WriteLine("Distinct ages:");
      // ? foreach (int age in distinctAges)
              Console.WriteLine(age);

      // ? IQueryable<string>? unique_distinct_first_names = _context.Students.Select(x => x.FirstName).Distinct();
      // ? List<string>? unique_distinct_first_names = await _context.Students.Select(x => x.FirstName).Distinct().ToListAsync();
      // ? Console.WriteLine("Distinct first names:");
      // ? foreach (string name in unique_distinct_first_names)
              Console.WriteLine(name);

*/
#endregion


#region All()
/**

  * * Determines whether all the elements of a sequence satisfy a condition.
  * * returns true if every element of the source sequence passes the test in the specified predicate, or if the sequence is empty; otherwise, false.
  * * Bir sorgu neticesinde gelen verilerin, verilen şarta uyup uymadığını kontrol etmektedir.
  * * Eğer ki tüm veriler şarta uyuyorsa true, uymuyorsa false döndürecektir.

      - bool are_all_students_male = _context.Students.AsQueryable().All(x => x.Gender == "Male");
      + Console.WriteLine(are_all_students_male);

      // ? Pet[] pets = { new Pet { Name="Barley", Age=10 },
      // ?                new Pet { Name="Boots", Age=4 },
      // ?                new Pet { Name="Bhiskers", Age=6 } };
      // ? bool allStartWithB = pets.AsQueryable().All(pet => pet.Name.StartsWith("B"));
      // ? Console.WriteLine("{0} pet names start with 'B'.",
      // ?             allStartWithB ? "All" : "Not all");
      // ? 
      // ? class Pet
      // ? {
      // ?   public string Name { get; set; }
      // ?   public int Age { get; set; }
      // ? }

*/
#endregion


#region Sum()
/**

  * * Computes the sum of a sequence of numeric values.
  * * Vermiş olduğumuz sayısal proeprtynin toplamını alır.

      - int total_ID = await _context.Students.AsQueryable().SumAsync(x => x.ID);

      // ? float?[] points = { null, 0, 92.83F, null, 100.0F, 37.46F, 81.1F };
      // ? float? sum = points.AsQueryable().Sum();
      // ? Console.WriteLine("Total points earned: {0}", sum);

*/
#endregion


#region Average()
/**

  * * Computes the average of a sequence of numeric values.
  * * Vermiş olduğumuz sayısal proeprtynin aritmatik ortalamasını alır.

      - double average_total_ID = await _context.Students.AsQueryable().AverageAsync(x => x.ID);

      // ? long?[] longs = { null, 10007L, 37L, 399846234235L };
      // ? double? average = longs.AsQueryable().Average();
      // ? Console.WriteLine("The average is {0}.", average);

*/
#endregion


#region Contains()
/**

  * * Determines whether an IQueryable<T> contains a specified element.
  * * returns true if the input sequence contains an element that has the specified value; otherwise, false.
  * * Like '%...%' sorgusu oluşturmamızı sağlar.

      - List<Student>? students = await _context.Students.Where(x => x.FirstName.Contains("Be")).ToListAsync();
      - foreach (var student in students)
            Console.WriteLine(student.FirstName);

      // ?       string[] fruits = { "apple", "banana", "mango",
      // ?                           "orange", "passionfruit", "grape" };
      // ? bool hasMango = fruits.AsQueryable().Contains("mango");
      // ? Console.WriteLine(hasMango);

*/
#endregion
#endregion
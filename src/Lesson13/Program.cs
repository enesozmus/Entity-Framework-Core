// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region ToDictionary() & ToArray() & Select() & SelectMany() & GroupBy()
#region ToDictionary()
/**

  * * Creates a Dictionary<TKey,TValue> from an IEnumerable<T> according to a specified key selector.
  * * Creates a Dictionary<TKey,TValue> from an IEnumerable<T> according to specified key selector and element selector functions.
  * * Sorgu neticesinde gelecek olan veriyi bir Dictionary<TKey,TValue> olarak karşılar.
  * * ToList() : Gelen sorgu neticesini entity türünde bir koleksiyona(List<TEntity>) dönüştürmekteyken,
  * * ToDictionary ise : Gelen sorgu neticesini Dictionary<TKey,TValue> türünden bir koleksiyona dönüştürecektir.

      - Dictionary<string, string>? students = await _context.Students.ToDictionaryAsync(x => x.LastName, x => x.FirstName);
          foreach (var student in students)
              Console.WriteLine(student);

*/
#endregion


#region ToArray()
/**

  * * Creates an array from a IEnumerable<T>.
  * * Sorgu neticesinde gelecek olan veriyi bir Array[] olarak karşılar.

      - Student[]? students = await _context.Students.ToArrayAsync();
            Console.WriteLine(students[0].FirstName);

*/
#endregion


#region Select()
/**

  * * Select fonksiyonunun işlevsel olarak birden fazla davranışı söz konusudur:
  * ? 1. Select fonksiyonu, generate edilecek sorgunun çekilecek kolonlarını ayarlamamızı sağlamaktadır. 

      - IQueryable<Student>? students =  _context.Students.Select(x => new Student
        {
          // bana sadece ID ve LastName property'lerini getir.
          ID = x.ID,
          LastName = x.LastName
        });

  * ? 2. Select fonksiyonu, gelen verileri farklı türlerde karşılayabilmemizi sağlar. T, anonim

      - var students = _context.Students.Select(x => new
        {
          ID = x.ID,
          LastName = x.LastName
        });
        
*/
#endregion


#region SelectMany()
/**

  * * Bir LINQ sorgusunda 2 farklı veri kaynağından seçilen kayıtlar (satırlar) ortak bir alana göre birleştirilerek tek bir kayıt elde edilebilir.
  * * Bunun için where cümleciğinde bu veri kaynaklarındaki kayıtların birleştirilmesi için kullanılacak ortak değer belirtilerek, uygun kayıtlardan yeni kayıtlar oluşturularak seçilir.
  * * 'from' cümleciği ile yardımıyla seçilecek veri kaynakları ayrı ayrı bildirilebilir.
  * * Bu işlem LINQ Yöntem Sözdizimi (LINQ Method Syntax)'nde SelectMany yöntemi aracılığıyla yapılır ve SQL Join cümleciğine karşılık gelir.
  * ? Select ile aynı amaca hizmet eder. Lakin, ilişkisel tablolar neticesinde gelen koleksiyonel verileri de tekilleştirip projeksiyon etmemizi sağlar.

      - var students = _context.Students.Include(l => l.Lessons).SelectMany(l => l.Lessons, (s, l) => new
        {
          s.FirstName,
          s.LastName,
          l.LessonName
        });

        foreach (var newIQueryable in students)
          Console.WriteLine(newIQueryable.LessonName);

*/
#endregion

#region GroupBy()
/**

  * * Groups the elements of a sequence.
  * * Groups the query results by the specified criteria.
  * * The GroupBy method is used to group results.

      - IQueryable<IGrouping<string, Student>>? datas = _context.Students.GroupBy(x => x.Gender);
      - var datas = _context.Students.GroupBy(x => x.Gender).Select(anonim => new
                  {
                    Count = anonim.Count(),
                    Gender = anonim.Key
                  });

        foreach (var item in datas)
          Console.WriteLine(item.Gender + " " + item.Count);

*/
#endregion
#endregion
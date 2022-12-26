// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


#region Fetching_Multiple_Objects

/**

  * * Birden çok nesneyle ilgili değerlerin alınmasına yönelik sorgular, yalnızca veriler iterate edildiğinde bir veritabanına karşı yürütülür.
  * * Bir foreach döngüsü veya sorguda ToList, Sum veya Count gibi bir sonlandırma yöntemi kullandığınızda veriler iterate edilir.

  * * define query

        - DbSet<Student>? students = _context.Students;
  
  * * query executed and data obtained from database

        - foreach(var student in students) {} Sorgu, foreach döngüsüne ulaşılana kadar yürütülmez.

  * * ToList()

        - Creates a List<T> from an IEnumerable<T>.
        - forces immediate query evaluation 
        - returns a List<T> that contains the query results
        - returns a List<T> that contains elements from the input sequence.
        - The ToList<TSource>(IEnumerable<TSource>) method forces immediate query evaluation and returns a List<T> that contains the query results. 
        - Anında yürütmeyi zorlar.  
        - Üretilen sorguyu execute ettirmemizi sağlayan fonksiyondur.
        - Birden çok varlığı döndürmek için kullanılan en yaygın yöntemdir.
        - Sorgu sonuçlarının önbelleğe alınmış bir kopyasını elde etmek için bu yöntemi sorgunuza ekleyebilirsiniz.

        - List<Student>? students = await _context.Students.ToListAsync();
        - List<Student>? students = (from student in _context.Students
                           select student).ToList();

*/
#endregion


#region Filtering

/**

  * * Filters a sequence of values based on a predicate.
  * * The Where method is the principal method for filtering results.
  * * The filter criteria are passed in to a lambda as an expression that returns a boolean.
  * * The expression can include multiple conditions.
  * * This method has at least one parameter of type Expression<TDelegate> whose type argument is one of the Func<T,TResult> types.
  * * For these parameters, you can pass in a lambda expression and it will be compiled to an Expression<TDelegate>.

  * * returns an IQueryable<T> that contains elements from the input sequence that satisfy the condition specified by predicate.

      - IQueryable<Student>? students = _context.Students.Where(p => p.ID == 1);
      - IQueryable<Student>? students = _context.Students.Where(p => p.ID == 1 && p.FirstName == "...");

      - List<Student>? students = (from student in _context.Students
                           where student.ID == 1 && student.FirstName == "..."
                           select student).ToList();

*/
#endregion


#region Ordering

/**

  * * OrderBy
  * * Sorgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur. (Ascending)
  * * Sorts the elements of a sequence in ascending order according to a key.
  * * This method has at least one parameter of type Expression<TDelegate> whose type argument is one of the Func<T,TResult> types.
  * * For these parameters, you can pass in a lambda expression and it will be compiled to an Expression<TDelegate>.

  * * returns an IOrderedQueryable<T> whose elements are sorted according to a key.


      - IOrderedQueryable<Student>? students = _context.Students.
                            Where(x => x.ID > 500 || x.FirstName.EndsWith("2")).
                            OrderBy(x => x.FirstName);

      - IOrderedQueryable<Student>? students = from student in _context.Students
                                              where student.ID > 500 || student.FirstName.StartsWith("...")
                                              orderby student.FirstName
                                              select student;
      - 

  * * ThenBy
  * * OrderBy üzerinde yapılan sıralama işlemini farklı kolonlarada uygulamamızı sağlayan bir fonksiyondur. (Ascending)
  * * Diyelim ki OrderBy yöntemini kullanarak bir sıralama işlemi yaptık ve (olası aynılık)tekrardan başka bir alan üzerinde de sıralama işlemi sağlamak istiyoruz.
  * * Bu işlemi gerçekleştirmek için de ThenBy ve ThenByDescending yöntemlerini kullanabiliyoruz.
  * * Performs a subsequent ordering of the elements in a sequence in ascending order.
  * * Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
  * * returns an IOrderedQueryable<T> whose elements are sorted according to a key.
  * * This method has at least one parameter of type Expression<TDelegate> whose type argument is one of the Func<T,TResult> types.
  * * For these parameters, you can pass in a lambda expression and it will be compiled to an Expression<TDelegate>.

*/
#endregion

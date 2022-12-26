// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region Fetching_Single_Object
#region Single() & SingleOrDefault()

/*

  * * Returns a single, specific element of a sequence.
  ? ? Expression<Func<TSource,Boolean>> 
  * * Returns the single element of a sequence that satisfies a specified condition,
  * * and throws an exception if there is more than one element in the sequence that satisfies the condition
  * * or if there no element in the sequence that satisfies the condition.

        - Unhandled exception. System.InvalidOperationException: Sequence contains more than one element
        - Unhandled exception. System.InvalidOperationException: Sequence contains no elements

  * * To instead return null when no matching element is found, use SingleOrDefault().

  * * Oluşturacağın sorguda koşulu sağlayan birden fazla sonuç varsa veya hiç yoksa her iki durumda da exception fırlatsın istiyorsan Single() yöntemini kullanabilirsin.

      - Student student = _context.Students.Single(x => x.FirstName == "Ben");

  * * Oluşturacağın sorguda koşulu sağlayan birden fazla sonuç varsa exception fırlatsın, hiç yoksa null dönsün istiyorsan SingleOrDefault() yöntemini kullanabilirsin.
  
      - Student student = _context.Students.SingleOrDefault(x => x.FirstName == "Ben");
      
*/
#endregion


#region First() & FirstOrDefault()

/*

  * * Returns the first element in a sequence that satisfies a specified condition.
  ? ? Expression<Func<TSource,Boolean>> 
  * * Throws an exception if there no element in the sequence that satisfies the condition.

        - Unhandled exception. System.InvalidOperationException: Sequence contains no elements

  * * To instead return a default value when no matching element is found, use the FirstOrDefault() method.

  * * Oluşturacağın sorguda koşulu sağlayan bir veya birden fazla sonuç varsa ve ilkini döndürsün istiyorsan ve koşulu sağlayan
  hiçbir değer olmadığında exception fırlatsın istiyorsan First() yöntemini kullanabilirsin.

      - Student student = _context.Students.First(x => x.FirstName == "Ben");

  * * Oluşturacağın sorguda koşulu sağlayan hiçbir değer olmadığında exception yerine null dönsün istiyorsan FirstOrDefault() yöntemini kullanabilirsin.
  
      - Student student = _context.Students.FirstOrDefault(x => x.FirstName == "Ben");
      
*/
#endregion


#region Find()

/**

    * * Finds an entity with the given primary key values.
    * * If no entity is found, then null is returned.
    * * Find fonksiyonu, primary key kolonuna özel hızlı bir şekilde sorgulama yapmamızı sağlayan bir fonksiyondur.

        - Student? student = _context.Students.Find(1);

*/
#endregion


#region Last() & LastOrDefault()

/*

  * * Returns the last element of a sequence that satisfies a specified condition.
  ? ? Expression<Func<TSource,Boolean>>
  * * Throws an exception if there no element in the sequence that satisfies the condition.

        - Unhandled exception. System.InvalidOperationException: Queries performing 'Last' operation must have a deterministic sort order. Rewrite the query to apply an 'OrderBy' operation on the sequence before calling 'Last'.
        - Unhandled exception. System.InvalidOperationException: Sequence contains no elements

  * * To instead return a default value when no matching element is found, use the LastOrDefault() method.   
*/
#endregion
#endregion
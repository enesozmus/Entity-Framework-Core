#region Method and Query Syntax

// var products = await context.Products.ToListAsync();

// var products2 = await (from product in context.Products
//                        select product).ToListAsync();

#endregion


#region IQueryable and IEnumerable Interfaces

// IEnumerable ve IQueryable, bir veri koleksiyonunu tutmak için kullanılır.
// Ayrıca iş gereksinimlerine göre filtreleme, sıralama, gruplama gibi veri işleme işlemlerini gerçekleştirmek için kullanılır.


#region What is IEnumerable interface?

/*

  * * IEnumerable<T> is the base interface forcollections in the System.Collections.Generic namespace.
  * * Collections that implement IEnumerable<T> can be enumerated by using the foreach statement.
  * * The IEnumerable is mostly used for LINQ to Object and LINQ to XML queries.
  * * The IEnumerable collection is of type forward only. That means it can only move in forward, it can’t move backward and between the items.
  * * IEnumerable supports deferred execution.
  * * It doesn’t support custom queries.
  * * The IEnumerable doesn’t support lazy loading. Hence, it is not suitable for paging-like scenarios.

  ? ? IEnumerable, EF Core üzerinden yapılmış olan sorgunun execute edilip verilerin in memory'ye yüklenmiş halini ifade eder.
  * * IEnumerable interface'nin ana amacı ortaya bir enumerator çıkarmaktır. Enumerator bir collection veya array'i yinelemek/iterate ettirmek içindir.
  * * IEnumerable<T> interface'ini implement eden herhangi bir Class, IEnumerable interface'nin GetEnumerator method'unu kullanabilir hale gelir.
  * * Bu method tüketicinin öğeler üzerinde dolaşabilmesini sağlar.
  * * Çok üst düzey bir interface olduğundan en iyi esnekliği sağlar.

*/

#endregion


#region What is IQueryable interface?

/*

  * * The IQueryable interface derives from the IEnumerable interface.
  * * IQueryable is mostly used for LINQ to SQL and LINQ to Entities queries.
  * * The collection of type IQueryable can move only forward, it can’t move backward and between the items.
  * * IQueryable supports deferred execution.
  * * It also supports custom queries using CreateQuery and Executes methods.
  * * IQueryable supports lazy loading and hence it is suitable for paging-like scenarios. 

  ? ? IQueryable, EF Core üzerinden yapılmış olan bir sorgunun execute edilmemiş halini ifade eder.
  * * IQueryable interface'nin temel amacı, belirli bir veri kaynağına göre verileri sorgulamak için işlevsellik sağlamaktır.

*/

#endregion


#endregion

#region IQueryable<Student> and Foreach

// IQueryable<Student>? students = from student in _context.Students
//                                 select student;


// foreach (Student student in students)
// {
//   Console.WriteLine(student.FirstName);
// }

#endregion

#region Deferred Execution(Ertelenmiş Çalışma)

/**

  * * IQueryable çalışmalarında ilgili kod yazıldığı noktada tetiklenmez/çalıştırılmaz yani ilgili kod yazıldığı noktada sorguyu generate etmez!
  * * Nerede eder? Çalıştırıldığı/execute edildiği noktada tetiklenir! İşte bu durumada ertelenmiş çalışma denir!

*/

// int first_param = 0;
// string second_param = "Ben";

// IQueryable<Student>? students = from student in _context.Students
//                                 where student.ID > first_param && student.FirstName.Contains(second_param)
//                                 select student;

// second_param = "Steve";

// foreach (Student student in students)
// {
//   Console.WriteLine(student.LastName);
// }

#endregion
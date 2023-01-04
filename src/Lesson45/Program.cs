Console.WriteLine("Hello, World!");

#region Query Tags
/**

  * * EF Core ile generate edilen sorgulara açıklama eklememizi sağlayarak; SQL Profiler, Query Log vs. gibi yapılarda bu açıklamalar eşliğinde sorguları gözlemleyebilmemizi sağlayan bir özelliktir.
  * * ToListAsync()/execute öncesi çağrılmalıdır.
*/
#region TagWith()

// await _context.Persons.TagWith("......açıklama...").ToListAsync();

// await _context.Persons.TagWith("Tüm personeller çekilmişit.r")
//    .Include(p => p.Orders).TagWith("Personellerin yaptığı satışlar sorguya eklenmiştir.")
//    .Where(p => p.Name.Contains("a")).TagWith("Adında 'a' harfi olan personeller filtrelenmiştir.")
//    .ToListAsync();
#endregion


#region TagWithCallSite()
/*
  * * Oluşturulan sorguya açıklama satırı ekler ve ek olarak bu sorgunun bu dosyada (.cs) hangi satırda üretildiğini bilgisini de verir.

    await _context.Persons.TagWithCallSite("Tüm personeller çekilmişit.r")
            .Include(p => p.Orders).TagWithCallSite("Personellerin yaptığı satışlar sorguya eklenmiştir.")
            .Where(p => p.Name.Contains("a")).TagWithCallSite("Adında 'a' harfi olan personeller filtrelenmiştir.")
            .ToListAsync();
*/
#endregion
#endregion
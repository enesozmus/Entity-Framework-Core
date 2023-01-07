#region Yalnızca İhtiyaç Olan Kolonları Listeleyin - Select
// var persons = await _context.Persons.Select(p => new
// {
//    p.Name
// }).ToListAsync();
#endregion


#region Result'ı Limitleyin - Take
// await _context.Persons.Take(50).ToListAsync();
#endregion


#region Join Sorgularında Eager Loading Sürecinde Verileri Filtreleyin
// var persons = await _context.Persons.Include(p => p.Orders
//                                                  .Where(o => o.OrderId % 2 == 0)
//                                                  .OrderByDescending(o => o.OrderId)
//                                                  .Take(4))
//                                    .ToListAsync();

// + bu işlem yukarıdaki gibi gerçekleştirilmeli
//foreach (var person in persons)
//{
//    var orders = person.Orders.Where(o => o.CreatedDate.Year == 2022);
//}
#endregion


#region Şartlara Bağlı Join Yapılacaksa Eğer Explicit Loading Kullanın
// ❌ var person = await context.Persons.Include(p => p.Orders).FirstOrDefaultAsync(p => p.PersonId == 1);
// ✅ var person = await context.Persons.FirstOrDefaultAsync(p => p.PersonId == 1);

// if (person.Name == "Ayşe")
// {
//    await context.Entry(person).Collection(p => p.Orders).LoadAsync();
// }
#endregion


#region Lazy Loading Kullanırken Dikkatli Olun!
#region Riskli Durum
// var persons = await context.Persons.ToListAsync();

// foreach (var person in persons)
// {
//    foreach (var order in person.Orders)
//    {
//        Console.WriteLine($"{person.Name} - {order.OrderId}");
//    }
//    Console.WriteLine("***********");
// }
#endregion
#region İdeal Durum
// var persons = await context.Persons.Select(p => new { p.Name, p.Orders }).ToListAsync();

// foreach (var person in persons)
// {
//    foreach (var order in person.Orders)
//    {
//        Console.WriteLine($"{person.Name} - {order.OrderId}");
//    }
//    Console.WriteLine("***********");
// }
#endregion
#endregion
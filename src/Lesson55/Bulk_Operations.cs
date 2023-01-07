#region New in Entity Framework 7: Bulk Operations with ExecuteDelete() and ExecuteUpdate()
/*

  * So why is this feature needed if we already can update and delete entities?
  ! The key word here is performance.
  * Instead of first retrieving the entities and having all the entities in memory before we can perform an action on them, and lastly committing them to SQL.
  + We now can do this with just a single operation, which results in one SQL command.

*/


#region EF Core 7 Öncesi Toplu Güncelleme
// var persons = await _context.Persons.Where(p => p.PersonId > 5).ToListAsync();
// foreach (var person in persons)
// {
//    person.Name = $"{person.Name}...";
// }
// await _context.SaveChangesAsync();
#endregion
#region EF Core 7 Öncesi Toplu Silme
// var persons = await _context.Persons.Where(p => p.PersonId > 5).ToListAsync();
// _context.RemoveRange(persons);
// await _context.SaveChangesAsync();
#endregion


#region ExecuteUpdate()
// ! ExecuteUpdate() ve ExecuteDelete() fonksiyonları ile bulk(toplu) veri güncelleme ve silme işlemleri gerçekleştirirken SaveChanges() fonksiyonunu çağırmamız gerekmemektedir.
// ! Çünkü bu fonksiyonlar adları üzerinde Execute... fonksiyonlarıdır. Yani direkt verittabanına fiziksel etkide bulunurlar.

// ✅ await _context.Persons.Where(p => p.PersonId > 3).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, v => v.Name + " yeni"));
// ❌ await _context.Persons.Where(p => p.PersonId > 3).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, v => $"{v.Name} yeni"));
#endregion

#region ExecuteDelete()
// await _context.Persons.Where(p => p.PersonId > 3).ExecuteDeleteAsync();
#endregion

#endregion
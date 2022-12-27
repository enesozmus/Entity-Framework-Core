// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region Change Tracking in EF Core
/**

  * * Every DbContext instance is automatically tracked for changes made to entities.
  * * These tracked entities in turn drive the changes to the database when SaveChanges is called.
  * * By default, EF Core creates a snapshot of every entity's property values when it is first tracked by a DbContext instance.
  * * The values stored in this snapshot are then compared against the current values of the entity in order to determine which property values have changed.
  * * The lifetime of a DbContext should be:

        1. Create the DbContext instance
        2. Track some entities
        3. Make some changes to the entities
        4. Call SaveChanges to update the database
        5. Dispose the DbContext instance

  * * Every entity is associated with a given EntityState:

      * ? Detached entities are not being tracked by the DbContext.
      * ? Added entities are new and have not yet been inserted into the database. This means they will be inserted when SaveChanges is called.
      * ? Unchanged entities have not been changed since they were queried from the database. All entities returned from queries are initially in this state.
      * ? Modified entities have been changed since they were queried from the database. This means they will be updated when SaveChanges is called.
      * ? Deleted entities exist in the database, but are marked to be deleted when SaveChanges is called.

*/
#endregion


#region ChangeTracker Property
/**

  * * An object used to access features that deal with change tracking.
  * * Provides access to information and operations for entity instances this context tracking.
  * * Context sınıfının base class'ı olan DbContext sınıfının bir member'ıdır.
  * * Takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekşetirmemizi sağlayan bir propertydir.

      var students = await _context.Students.ToListAsync();
          → students[1].FirstName = "demo";           → Modified
          → _context.Students.Remove(students[6]);    → Deleted
      
      var datas = _context.ChangeTracker.Entries();

      foreach (var item in datas)
        Console.WriteLine(item.State);

*/
#endregion


#region DetectChanges()
/**

  * * By default, EF Core creates a snapshot of every entity's property values when it is first tracked by a DbContext instance.
  * * The values stored in this snapshot are then compared against the current values of the entity in order to determine which property values have changed.
  * * This detection of changes happens when SaveChanges is called to ensure all changed values are detected before sending updates to the database.
  * * However, the detection of changes also can happen at other times to ensure the application is working with up-to-date tracking information. 
  * * Detection of changes can be forced at any time by calling ChangeTracker.DetectChanges().
  * * Yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişişiklerin algılanmasını opsiyonel olarak gerçekleştirmek isteyebiliriz.
  * * İşte bunun için DetectChanges fonksiyonu kullanılabilir ve her ne kadar EF Core değişikleri otomatik algılıyor olsa da siz yine de iradenizle kontrole zorlayabilirsiniz.

      - _context.ChangeTracker.DetectChanges();

*/
#endregion


#region ChangeTracker.AutoDetectChangesEnabled
/**

  * * The performance of detecting changes is not a bottleneck for most applications.
  * * However, detecting changes can become a performance problem for some applications that track thousands of entities.
  * * For this reason the automatic detection of changes can be disabled using ChangeTracker.AutoDetectChangesEnabled.
  * * DetectChanges fonksiyonunun, otomatik izleme, kullanımını irademizle yönetmek ve maliyet/performans optimizasyonu yapmak istediğimiz durumlarda AutoDetectChangesEnabled property'sini kapatabiliriz.

      - _context.ChangeTracker.AutoDetectChangesEnabled = false;
      - _context.ChangeTracker.AutoDetectChangesEnabled = true;

*/
#endregion


#region Entries()
/**

  * * Returns an EntityEntry for each entity being tracked by the context.
  * * The entries provide access to change tracking information and operations for each entity.
  * * This method calls DetectChanges() to ensure all entries returned reflect up-to-date state.
  * * Use AutoDetectChangesEnabled to prevent DetectChanges from being called automatically.
  * * Context'te ki Entry metodunun koleksiyonel versiyonudur.
  * * Change Tracker mekanizması tarafından izlenen her entity nesnesinin bigisini EntityEntry türünden elde etmemizi sağlar.
  * * Entries() metodu, DetectChanges() metodunu tetikler. Bu durum da tıpkı SaveChanges'da olduğu gibi bir maliyettir.

      - _context.ChangeTracker.Entries().ToList().ForEach(e =>
      {
          if (e.State == EntityState.Unchanged)
          {
            //...
          }
          else if (e.State == EntityState.Deleted)
          {
            //...
          }
      });

*/
#endregion


#region AcceptAllChanges()
/**

  * * Accepts all changes made to entities in the context. It will be assumed that the tracked entities represent the current state of the database.
  * * This method is typically called by SaveChanges() after changes have been successfully saved to the database.
  * * The AcceptAllChanges method is automatically called if no exception is thrown when updating, which will reset the state of all objects to Unchanged.
  * * The AcceptAllChanges method is useful in the scenario where a transaction has failed and a user wants to retry.

  * * SaveChanges(false) tells the EF to execute the necessary database commands, but hold on to the changes, so they can be replayed if necessary.

  * * SaveChanges() tetiklendiğinde EF Core her şeyin yolunda olduğunu varsayarak track ettiği verilerin takibini bırakır ve yeni değişikliklerin takip edilmesini bekler.
  * * Böyle bir durumda beklenmeyen bir durum/olası bir hata söz konusu olursa EF Core takip ettiği nesneleri takip etmeyi bıraktığı için bir 'düzeltme yapabilme' ihtimali söz konusu olmayacaktır.
  * * Haliyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges() metotları girecektir.

  * * SaveChanges(False), EF Core'a gerekli veritabanı komutlarını yürütmesini söyler ancak gerektiğinde yeniden oynatılabilmesi için değişikleri beklemeye/nesneleri takip etmeye devam eder.
  * * Taa ki AcceptAllChanges metodunu irademizle çağırana kadar! SaveChanges(false) ile işlemin başarılı olduğundan emin olursanız AcceptAllChanges metodu ile nesnelerden takibi kesebilirsiniz.

        - _context.ChangeTracker.AcceptAllChanges();
        
*/
#endregion


#region HasChanges()
/**

  * * Checks if any new, deleted, or changed entities are being tracked such that these changes will be sent to the database if SaveChanges() or SaveChangesAsync(CancellationToken) is called.
  * * Returns true if there are changes to save, otherwise false.
  * * Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisini verir.
  * * Arkaplanda DetectChanges metodunu tetikler.

        - _context.ChangeTracker.HasChanges();

*/
#endregion


#region DbContext.Entry()
// Gets a DbEntityEntry object for the given entity providing access to information about the entity and the ability to perform actions on the entity.

//    - _context.Entry(object entity);
//    - _context.Entry<Student>(object entity);
#endregion


#region DbContext.Entry().OriginalValues. & DbContext.Entry().CurrentValues.
// Gets the original or current property values for this entity.
//    - _context.Entry(student).OriginalValues.
//    - _context.Entry(student).OriginalValues.GetValue<string>(nameof(student.FirstName));
//    - _context.Entry(student).CurrentValues.
#endregion


#region DbContext.Entry().GetDatabaseValues()
// Queries the database for copies of the values of the tracked entity as they currently exist in the database.
// If the entity is not found in the database, then null is returned.
//    - _context.Entry(student).GetDatabaseValues();
#endregion


#region Using Change Tracker as Interceptor
// public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
// {
//   var entries = ChangeTracker.Entries();
//   foreach (var entry in entries)
//   {
//     if (entry.State == EntityState.Added)
//     {

//     }
//   }
//   return base.SaveChangesAsync(cancellationToken);
// }
#endregion

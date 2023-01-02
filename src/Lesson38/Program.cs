Console.WriteLine("Hello, World!");

#region Complex Query Operators
/**

    * * Language Integrated Query (LINQ) contains many complex operators, which combine multiple data sources or does complex processing.
    * ! Not all LINQ operators have suitable translations on the server side. 
    * * Sometimes, a query in one form translates to the server but if written in a different form doesn't translate even if the result is the same.
    * * A particular query, which is translated in SqlServer, may not work for SQLite databases.   
    * ? DefaultIfEmpty() : Sorgulama sürecinde ilişkisel olarak karşılığı olmayan verilere default değerini yazdıran bir fonksiyondur.
 
*/
#endregion


#region Join() → SQL [Inner Join]
/**

    * * The LINQ Join operator allows you to connect two data sources based on the key selector for each source, generating a tuple of values when the key matches.
    * * It naturally translates to INNER JOIN on relational databases. 

*/
/** Example 1
    * * PersonPhoto & Person:

    * * query syntax
        var query = from photo in context.Set<PersonPhoto>()
                    join person in context.Set<Person>()
                        on photo.PersonPhotoId equals person.PhotoId
                    select new { person, photo };
                    // or
                    select new { person.Name, photo.Url }; 
        // query.ToList();

    * * method syntax
        var query = context.Photos.Join
                            (
                                context.Persons,
                                photo => photo.PersonId, person => person.PersonId,
                            (photo, person) => new
                            {
                                person.Name,
                                photo.Url
                            });
*/
/** Example 2

    * * The following query joins Person and EmailAddresses table using the join Query operator.
    * * The Join operator uses the 'Equals Keyword' to compare the specified properties.
    * * The query begins with from p in db.People which is the outer table in our join.
    * * We then use the 'join keyword' to join the inner table. (join e in db.EmailAddresses)
    * * The 'on keyword' is used to specify the join condition.
    * * The first part of the condition should always from the outer table (i.e People).
    * ! We use the 'equals keyword' to compare the conditions. Also, we can only compare for equality. Other comparisons are not supported.

        using (AdventureWorks _context = new AdventureWorks())
        {
            var person = (from p in _context.People
                            join e in _context.EmailAddresses
                            on p.BusinessEntityID equals e.BusinessEntityID
                            where p.FirstName == "KEN"
                        select new
                        {
                            ID = p.BusinessEntityID, 
                            FirstName = p.FirstName, 
                            MiddleName = p.MiddleName, 
                            LastName = p.LastName, 
                            EmailID = e.EmailAddress1 
                        }).ToList();

            var person = _context.People.Join
                        (
                            _context.EmailAddresses, 
                            p => p.BusinessEntityID, e => e.BusinessEntityID,
                            (p, e) => new { 
                                FirstName = p.FirstName, 
                                MiddleName = p.MiddleName, 
                                LastName = p.LastName, 
                                EmailID = e.EmailAddress1 }
                        ).Take(5);
        }
*/
#endregion


#region Join() → SQL [Inner Join] - on Multiple Columns
/**

    * * To use join on more than one columns (Join by using composite keys), we need to define an anonymous type with the values we want to compare
    * * Query Syntax

        var query = from photo in _context.Photos
                    join person in _context.Persons
                        on new { photo.PersonId, photo.Url } equals new { person.PersonId, Url = person.Name }
                    select new
                    {
                        person.Name,
                        photo.Url
                    };

    * ! works only if  the data types and the names of the properties in the anonymous types match

        var result = (from m1 in db.model1
              join m2 in db.model2
               on new { m1.field1 , m1.field2 } 
               equals new { m2.field1, m2.field2 }
             where m1.FirstName == "KEN"
             select new
              {
                field1 = m1.field1,
                field2 = m1.field2,
                someField = m2.someField
             }).ToList();

    * * Method Syntax

        var query = context.Photos .Join
                            (
                                context.Persons,
                                photo => new { photo.PersonId, photo.Url },
                                person => new { person.PersonId, Url = person.Name },
                            (photo, person) => new
                            {
                                person.Name,
                                photo.Url
                            });
*/
#endregion


#region Group Join
//var query = from person in context.Persons
//            join order in context.Orders
//                on person.PersonId equals order.PersonId into personOrders
//            //from order in personOrders
//            select new
//            {
//                person.Name,
//                Count = personOrders.Count(),
//                personOrders
//            };
#endregion

#region Left Join
//var query = from person in context.Persons
//            join order in context.Orders
//                on person.PersonId equals order.PersonId into personOrders
//            from order in personOrders.DefaultIfEmpty()
//            select new
//            {
//                person.Name,
//                order.Description
//            };

//var datas = await query.ToListAsync();
#endregion

#region Right Join
//var query = from order in context.Orders
//            join person in context.Persons
//                on order.PersonId equals person.PersonId into orderPersons
//            from person in orderPersons.DefaultIfEmpty()
//            select new
//            {
//                person.Name,
//                order.Description
//            };

//var datas = await query.ToListAsync();
#endregion

#region Full Join
//var leftQuery = from person in context.Persons
//                join order in context.Orders
//                    on person.PersonId equals order.PersonId into personOrders
//                from order in personOrders.DefaultIfEmpty()
//                select new
//                {
//                    person.Name,
//                    order.Description
//                };


//var rightQuery = from order in context.Orders
//                 join person in context.Persons
//                     on order.PersonId equals person.PersonId into orderPersons
//                 from person in orderPersons.DefaultIfEmpty()
//                 select new
//                 {
//                     person.Name,
//                     order.Description
//                 };

// var fullJoin = leftQuery.Union(rightQuery);
// var datas = await fullJoin.ToListAsync();
#endregion

#region Cross Join  
// from ... from
//var query = from order in context.Orders
//            from person in context.Persons
//            select new
//            {
//                order,
//                person
//            };

//var datas = await query.ToListAsync();
#endregion

#region Collection Selector'da Where Kullanma Durumu
//var query = from order in context.Orders
//            from person in context.Persons.Where(p => p.PersonId == order.PersonId)
//            select new
//            {
//                order,
//                person
//            };

//var datas = await query.ToListAsync();
#endregion

#region Cross Apply
//var query = from person in context.Persons
//            from order in context.Orders.Select(o => person.Name)
//            select new
//            {
//                person,
//                order
//            };

//var datas = await query.ToListAsync();
#endregion

#region Outer Apply
//var query = from person in context.Persons
//            from order in context.Orders.Select(o => person.Name).DefaultIfEmpty()
//            select new
//            {
//                person,
//                order
//            };

//var datas = await query.ToListAsync();
#endregion

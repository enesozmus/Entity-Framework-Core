/**

  * * Cascade behaviors are configured per relationship using the OnDelete method in OnModelCreating.
  * * OnDelete accepts a value from the, admittedly confusing, DeleteBehavior enum.
  * * This enum defines both the behavior of EF Core on tracked entities, and the configuration of cascade delete in the database when EF is used to create the schema.
  * * : .OnDelete(DeleteBehavior.*******);

      DeleteBehavior	  Impact on database schema
      ---               ---
      Cascade	          ON DELETE CASCADE
      Restrict	        ON DELETE RESTRICT
      NoAction	        database default
      SetNull	          ON DELETE SET NULL
      ClientSetNull	    database default
      ClientCascade	    database default
      ClientNoAction	  database default

  * * The behaviors of ON DELETE NO ACTION (the database default) and ON DELETE RESTRICT in relational databases are typically either identical or very similar.
  * * Despite what NO ACTION may imply, both of these options cause referential constraints to be enforced.
  * * SQL Server doesn't support ON DELETE RESTRICT, so ON DELETE NO ACTION is used instead.

  * * The only values that will cause cascading behaviors on the database are Cascade and SetNull.
  * * All other values will configure the database to not cascade any changes.

  Required relationship with dependents/children loaded
  DeleteBehavior	    On deleting principal/parent	      On severing from principal/parent
  ---                 ---                                 ---
  Cascade	            Dependents deleted by EF Core	      Dependents deleted by EF Core
  Restrict	          InvalidOperationException	          InvalidOperationException
  NoAction	          InvalidOperationException	          InvalidOperationException
  SetNull	            SqlException on creating database	  SqlException on creating database
  ClientSetNull	      InvalidOperationException	          InvalidOperationException
  ClientCascade	      Dependents deleted by EF Core	      Dependents deleted by EF Core
  ClientNoAction	    DbUpdateException	                  InvalidOperationException
*/

/** Notes:

  * * The default for required relationships is 'Cascade'.
  * * Using anything other than cascade delete for required relationships will result in an exception when SaveChanges is called:
        - Typically, this is an InvalidOperationException from EF Core since the invalid state is detected in the loaded children/dependents.
  * ! ClientNoAction forces EF Core to not check fixup dependents before sending them to the database, so in this case the database throws an exception, which is then wrapped in a DbUpdateException by SaveChanges.
  * ? SetNull is rejected when creating the database since the foreign key column is not nullable.


*/
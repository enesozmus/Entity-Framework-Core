using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ORM_SQL_Isolation.Models;


#region without ORM

SqlConnection connection = new($"Data Source=DESKTOP-OPFJQHD; Database=Northwind; Integrated Security=True;");
await connection.OpenAsync();


// SqlCommand command = new(@"", connection);
// SqlCommand command = new(@"SELECT * FROM Categories;", connection);

/*
  * Veri erişimine yönelik bu yaklaşım hataya fazlasıyla açıktır.
  * Sorunlar genellikle bir sütunun adının yanlış yazılmasından veya sütun adının veritabanında değiştirilmiş olduğunun bulunmasından kaynaklanır.
*/
SqlCommand command = new(@"
SELECT employees.FirstName, products.ProductName, COUNT(*) [Count] FROM Employees employees
	INNER JOIN ORDERS orders
		ON employees.EmployeeID = orders.EmployeeID
	INNER JOIN [Order Details] orderDetails
		ON orders.OrderID = orderDetails.OrderID
	INNER JOIN Products products
		ON orderDetails.ProductID = products.ProductID
GROUP By employees.FirstName, products.ProductName
ORDER By COUNT(*) DESC;", connection);


SqlDataReader dr = command.ExecuteReader();


while (await dr.ReadAsync())
{
  // Console.WriteLine($"{dr["CategoryName"]}");
  Console.WriteLine($"{dr["FirstName"]} {dr["ProductName"]} {dr["Count"]}");
}

await connection.CloseAsync();

#endregion


#region with ORM

NorthwindContext context = new();

var query = context.Employees
                    .Include(employee => employee.Orders)
                    .ThenInclude(order => order.OrderDetails)
                    .ThenInclude(orderDetail => orderDetail.Product)
                    .SelectMany(employee => employee.Orders, (employee, order) => new { employee.FirstName, order.OrderDetails })
                    .SelectMany(data => data.OrderDetails, (data, orderDetail) => new { data.FirstName, orderDetail.Product.ProductName })
                    .GroupBy(data => new
                    {
                      data.ProductName,
                      data.FirstName
                    }).Select(data => new
                    {
                      data.Key.FirstName,
                      data.Key.ProductName,
                      Count = data.Count()
                    });

var data = await query.ToListAsync();
#endregion
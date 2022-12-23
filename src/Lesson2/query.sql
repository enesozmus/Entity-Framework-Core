-- Hangi personel hangi urunden kac adet satmistir?

--SELECT * FROM Employees employees
--	INNER JOIN ORDERS orders
--		ON employees.EmployeeID = orders.EmployeeID
--	INNER JOIN [Order Details] orderDetails
--		ON orders.OrderID = orderDetails.OrderID
--	INNER JOIN Products products
--		ON orderDetails.ProductID = products.ProductID;

SELECT employees.FirstName, products.ProductName, COUNT(*) [Count] FROM Employees employees
	INNER JOIN ORDERS orders
		ON employees.EmployeeID = orders.EmployeeID
	INNER JOIN [Order Details] orderDetails
		ON orders.OrderID = orderDetails.OrderID
	INNER JOIN Products products
		ON orderDetails.ProductID = products.ProductID
GROUP By employees.FirstName, products.ProductName
ORDER By COUNT(*) DESC;

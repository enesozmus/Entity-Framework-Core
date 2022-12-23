namespace ORM_SQL_Isolation.Models
{
  public partial class ProductsAboveAveragePrice
  {
    public string ProductName { get; set; } = null!;
    public decimal? UnitPrice { get; set; }
  }
}

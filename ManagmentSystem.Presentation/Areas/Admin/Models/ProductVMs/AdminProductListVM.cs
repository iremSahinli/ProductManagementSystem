namespace ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs
{
    public class AdminProductListVM
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryNames { get; set; }
        public string ProductImage { get; set; } //ürün görsellerini görebilmek için gerekiyor.
    }
}

namespace ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs
{
    public class AdminProductDetailVM
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public IEnumerable<string> CategoryNames { get; set; } // Kategorileri liste olarak tutar.
        public string CategoryName { get; set; }
    }
}

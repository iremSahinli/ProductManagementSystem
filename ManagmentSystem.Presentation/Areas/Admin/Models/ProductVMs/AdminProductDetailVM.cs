namespace ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs
{
    public class AdminProductDetailVM
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        //public List<string> CategoryName { get; set; } = new List<string>();
        public string CategoryName { get; set; }
    }
}

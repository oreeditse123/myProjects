namespace devDynast.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; } 
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ProductName { get; set; } 
        public string ProductImage { get; set; } 
         public string Category { get; set; } 
    }
}



using System;

namespace devDynast.ViewModels
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; } 
        public string? ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string? ProductImage { get; set; }
        public string? Category { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Status { get; set; }
    }

}

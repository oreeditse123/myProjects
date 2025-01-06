namespace devDynast.Models
{
    public class SalesDataViewModel
{
    public string? Month { get; set; }
    public int SalesCount { get; set; }
    public int? TopDay { get; set; } // Nullable in case there are no sales
    public int MaxSales { get; set; } // Maximum sales on the top day
}

}

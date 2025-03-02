namespace devDynast.Models
{
public class SalesCompareViewModel
{
    // If you're grouping by ProductId
    public string? ProductId { get; set; }

    // If you're grouping by week
    public string? GroupedBy { get; set; } 

    // Total count of items sold
    public int SalesCount { get; set; }

    // Total revenue generated
    public double TotalRevenue { get; set; }
    public int? Month { get; set; }

    // Group by a specific date or time (formatted for days or weeks)
    public string? DateGroup { get; set; }

    // Add any other fields you may want, like Month, Day, or Week
}

}

namespace ShopPC.DTO.Response
{
    public class CategoryRevenueItemResponse
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class CategoryRevenueResponse
    {
        public IEnumerable<CategoryRevenueItemResponse> Categories { get; set; } = new List<CategoryRevenueItemResponse>();
        public decimal TotalRevenue { get; set; }
    }
}

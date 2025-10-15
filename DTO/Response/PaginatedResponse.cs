﻿namespace ShopPC.DTO.Response
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}

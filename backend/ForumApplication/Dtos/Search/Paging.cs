using System;
using System.Collections.Generic;
using System.Linq;
namespace ForumApplication.Dtos.Search
{
    public class Paging
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemsCount { get; set; }
        public int TotalPages { get; set; }
        public double TotalDuration { get; set; }
        public double? QueryDuration { get; set; }
        public double? CountDuration { get; set; }
        public double? BuildingQueryDuration { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObjectComment
    {
        public string Symbol { get; set; }
        public string SortBy { get; set; } = "CreatedOn";
        public bool IsDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
    }
}
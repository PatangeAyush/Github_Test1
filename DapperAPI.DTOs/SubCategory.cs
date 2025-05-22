using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPI.DTOs
{
    public class SubCategory
    {
        public int SubCategory_ID { get; set; } 
        public string SubCategory_Name { get; set; } = string.Empty; 
        public int Master_SubCategory_ID { get; set; } 
        public string? Master_SubCategory_Name { get; set; } 
        public string? UrlPath { get; set; }
        public string? Icon { get; set; }
        public string? Banner { get; set; }
        public bool IsPublished { get; set; }
        public bool IsIncludeMenu { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

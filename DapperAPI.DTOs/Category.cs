using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPI.DTOs
{
    public class Category
    {
        public int Category_ID { get; set; } 
        public string Category_Name { get; set; } = string.Empty; 
        public string? UrlPath { get; set; } 
        public string? Icon { get; set; } 
        public string? Banner { get; set; } 
        public bool IsPublished { get; set; } 
        public bool IsIncludeMenu { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; }
    }
}

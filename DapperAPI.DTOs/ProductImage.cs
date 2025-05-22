using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPI.DTOs
{
    public class ProductImage
    {
        public int Image_ID { get; set; } 
        public int Product_ID { get; set; } 
        public string Image_Path { get; set; } = string.Empty; 
        public bool IsPrimary { get; set; } 
        public int SortOrder { get; set; } 
        public DateTime CreatedDate { get; set; } 
    }
}

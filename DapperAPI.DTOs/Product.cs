using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPI.DTOs
{
    public class Product
    {
        public int Product_ID { get; set; } 
        public string Product_Name { get; set; } = string.Empty;
        public string? Product_Icon { get; set; }
        public string? Product_Code { get; set; }
        public int MasterCategory_ID { get; set; }
        public string? Category_Name { get; set; } 
        public int MasterSubCategory_ID { get; set; }
        public string? Master_SubCategory_Name { get; set; } 
        public int SubCategory_ID { get; set; }
        public string? SubCategory_Name { get; set; } 
        public string? Short_Description { get; set; }
        public string? Long_Description { get; set; }
        public decimal Price { get; set; }
        public decimal? MRP { get; set; }
        public decimal? Discount_Percent { get; set; } // Changed to decimal to match SP
        public int Stock_Quantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // List to hold associated images
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}

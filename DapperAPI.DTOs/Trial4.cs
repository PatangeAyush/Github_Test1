using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPI.DTOs
{
    internal class Trial4
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public string? Product_Icon { get; set; }
        public string? Product_Code { get; set; }
        public int MasterCategory_ID { get; set; }
        public string? Category_Name { get; set; }
    }
}

using DapperAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPi.Repositorys
{
    public interface IMasterSubCategoryRepository
    {
        Task<IEnumerable<MasterSubCategory>> GetMasterSubCategoriesAsync();
        Task AddMasterSubCategoryAsync(MasterSubCategory masterSubCategory);
        Task UpdateMasterSubCategoryAsync(MasterSubCategory masterSubCategory);
        Task DeleteMasterSubCategoryAsync(int masterSubCategoryId);
    }
}

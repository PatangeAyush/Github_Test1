using Dapper;
using DapperAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPi.Repositorys
{
    public class MasterSubCategoryRepository: IMasterSubCategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public MasterSubCategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<MasterSubCategory>> GetMasterSubCategoriesAsync()
        {
          
            return await _dbConnection.QueryAsync<MasterSubCategory>(
                "SP_MasterSubCategory",
                new { action = "GetMasterSubCategories" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddMasterSubCategoryAsync(MasterSubCategory msc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@action", "AddMasterSubCategory");
            parameters.Add("@Master_SubCategory_Name", msc.Master_SubCategory_Name);
            parameters.Add("@Category_ID", msc.Category_ID);
            parameters.Add("@URLPath", msc.UrlPath);
            parameters.Add("@Icon", msc.Icon);
            parameters.Add("@Banner", msc.Banner);
            parameters.Add("@IsPublished", msc.IsPublished);
            parameters.Add("@IsIncludeMenu", msc.IsIncludeMenu);

            await _dbConnection.ExecuteAsync(
                "SP_MasterSubCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateMasterSubCategoryAsync(MasterSubCategory msc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@action", "UpdateMasterSubCategory");
            parameters.Add("@Master_SubCategory_ID", msc.Master_SubCategory_ID);
            parameters.Add("@Master_SubCategory_Name", msc.Master_SubCategory_Name);
            parameters.Add("@Category_ID", msc.Category_ID);
            parameters.Add("@URLPath", msc.UrlPath);
            parameters.Add("@Icon", msc.Icon);
            parameters.Add("@Banner", msc.Banner);
            parameters.Add("@IsPublished", msc.IsPublished);
            parameters.Add("@IsIncludeMenu", msc.IsIncludeMenu);

            await _dbConnection.ExecuteAsync(
                "SP_MasterSubCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteMasterSubCategoryAsync(int masterSubCategoryId)
        {
            await _dbConnection.ExecuteAsync(
                "SP_MasterSubCategory",
                new { action = "DeleteMasterSubCategory", Master_SubCategory_ID = masterSubCategoryId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

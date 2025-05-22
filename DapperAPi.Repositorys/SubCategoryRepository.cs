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
    public class SubCategoryRepository:ISubCategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public SubCategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
        {
            return await _dbConnection.QueryAsync<SubCategory>(
               "SP_SubCategory",
               new { action = "GetSubCategories" },
               commandType: CommandType.StoredProcedure
           );
        }

        public async Task AddSubCategoryAsync(SubCategory sc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@action", "AddSubCategory");
            parameters.Add("@SubCategory_Name", sc.SubCategory_Name);
            parameters.Add("@Master_SubCategory_ID", sc.Master_SubCategory_ID);
            parameters.Add("@URLPath", sc.UrlPath);
            parameters.Add("@Icon", sc.Icon);
            parameters.Add("@Banner", sc.Banner);
            parameters.Add("@IsPublished", sc.IsPublished);
            parameters.Add("@IsIncludeMenu", sc.IsIncludeMenu);

            await _dbConnection.ExecuteAsync(
                "SP_SubCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateSubCategoryAsync(SubCategory sc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@action", "UpdateSubCategory");
            parameters.Add("@SubCategory_ID", sc.SubCategory_ID);
            parameters.Add("@SubCategory_Name", sc.SubCategory_Name);
            parameters.Add("@Master_SubCategory_ID", sc.Master_SubCategory_ID);
            parameters.Add("@URLPath", sc.UrlPath);
            parameters.Add("@Icon", sc.Icon);
            parameters.Add("@Banner", sc.Banner);
            parameters.Add("@IsPublished", sc.IsPublished);
            parameters.Add("@IsIncludeMenu", sc.IsIncludeMenu);

            await _dbConnection.ExecuteAsync(
                "SP_SubCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteSubCategoryAsync(int subCategoryId)
        {
            await _dbConnection.ExecuteAsync(
               "SP_SubCategory",
               new { action = "DeleteSubCategory", SubCategory_ID = subCategoryId },
               commandType: CommandType.StoredProcedure
           );
        }
    }
}

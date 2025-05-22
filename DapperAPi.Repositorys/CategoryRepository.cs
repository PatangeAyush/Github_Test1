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
    public class CategoryRepository: ICategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public CategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _dbConnection.QueryAsync<Category>(
                "SP_CategoryMaster",
                new { action = "GetCategories" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddCategoryAsync(Category category)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@action", "AddCategory");
            parameters.Add("@Category_Name", category.Category_Name);
            parameters.Add("@URLPath", category.UrlPath);
            parameters.Add("@Icon", category.Icon);
            parameters.Add("@Banner", category.Banner);
            parameters.Add("@IsPublished", category.IsPublished);
            parameters.Add("@IsIncludeMenu", category.IsIncludeMenu);

            await _dbConnection.ExecuteAsync(
                "SP_CategoryMaster",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@action", "UpdateCategory");
            parameters.Add("@Category_ID", category.Category_ID);
            parameters.Add("@Category_Name", category.Category_Name);
            parameters.Add("@URLPath", category.UrlPath);
            parameters.Add("@Icon", category.Icon); 
            parameters.Add("@Banner", category.Banner); 
            parameters.Add("@IsPublished", category.IsPublished);
            parameters.Add("@IsIncludeMenu", category.IsIncludeMenu);

            await _dbConnection.ExecuteAsync(
                "SP_CategoryMaster",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _dbConnection.ExecuteAsync(
                "SP_CategoryMaster",
                new { action = "DeleteCategory", Category_ID = categoryId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

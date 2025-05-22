using Dapper;
using DapperAPI.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperAPi.Repositorys
{
    public class ProductRepository: IProductRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<ProductRepository> _logger; 

        public ProductRepository(IDbConnection dbConnection, ILogger<ProductRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger; 
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var multi = await _dbConnection.QueryMultipleAsync(
                "SP_ManageProduct",
                new { Action = "GetAllProducts" },
                commandType: CommandType.StoredProcedure
            );

            var products = (await multi.ReadAsync<Product>()).ToList();
            var images = (await multi.ReadAsync<ProductImage>()).ToList();

            var productDict = products.ToDictionary(p => p.Product_ID);
            foreach (var image in images)
            {
                if (productDict.TryGetValue(image.Product_ID, out var product))
                {
                    product.Images.Add(image);
                }
            }
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var multi = await _dbConnection.QueryMultipleAsync(
              "SP_ManageProduct",
              new { Action = "GetProductByID", ProductID = productId },
              commandType: CommandType.StoredProcedure
          );

            var product = await multi.ReadFirstOrDefaultAsync<Product>();
            if (product != null)
            {
                product.Images = (await multi.ReadAsync<ProductImage>()).ToList();
            }
            return product;
        }

        public async Task<int?> AddProductAsync(Product product)
        {
            var productParameters = new DynamicParameters();
            productParameters.Add("@Action", "AddProduct");
            productParameters.Add("@ProductName", product.Product_Name);
            productParameters.Add("@ProductIcon", product.Product_Icon); 
            productParameters.Add("@ProductCode", product.Product_Code);
            productParameters.Add("@MasterCategoryID", product.MasterCategory_ID);
            productParameters.Add("@MasterSubCategoryID", product.MasterSubCategory_ID);
            productParameters.Add("@SubCategoryID", product.SubCategory_ID);
            productParameters.Add("@ShortDescription", product.Short_Description);
            productParameters.Add("@LongDescription", product.Long_Description);
            productParameters.Add("@Price", product.Price);
            productParameters.Add("@MRP", product.MRP);
            productParameters.Add("@DiscountPercent", product.Discount_Percent);
            productParameters.Add("@StockQuantity", product.Stock_Quantity);
            productParameters.Add("@IsFeatured", product.IsFeatured);
            productParameters.Add("@IsAvailable", product.IsAvailable);

            int? newProductId = null;

            try
            {
                _logger?.LogInformation("Attempting to add product: {ProductName}", product.Product_Name);
                var productResult = await _dbConnection.QuerySingleOrDefaultAsync<dynamic>(
                    "SP_ManageProduct",
                    productParameters,
                    commandType: CommandType.StoredProcedure
                );

                if (productResult == null)
                {
                    _logger?.LogWarning("SP_ManageProduct returned null result when adding product master details for {ProductName}.", product.Product_Name);
                    return null;
                }

                var productResultDict = (IDictionary<string, object>)productResult;
                bool productAddedSuccessfully = false;

                if (productResultDict.TryGetValue("Success", out object? successValue))
                {
                    if ((successValue is bool boolVal && boolVal) || (successValue is int intVal && intVal == 1))
                    {
                        productAddedSuccessfully = true;
                    }
                }

                if (productAddedSuccessfully && productResultDict.TryGetValue("ProductID", out object? idValue) && idValue is int id)
                {
                    newProductId = id;
                    _logger?.LogInformation("Product {ProductName} added successfully with ID: {ProductId}", product.Product_Name, newProductId.Value);
                }
                else
                {
                    string failureReason = "Unknown error adding product master details.";
                    if (productResultDict.TryGetValue("Result", out object? reasonValue) && reasonValue != null)
                    {
                        failureReason = reasonValue.ToString() ?? failureReason;
                    }
                    else if (!productAddedSuccessfully)
                    {
                        failureReason = "Stored procedure indicated failure (Success flag false or missing) for product master.";
                    }
                    else
                    {
                        failureReason = "Stored procedure indicated success for product master, but ProductID was missing or invalid.";
                    }
                    _logger?.LogWarning("Failed to add product master details for {ProductName} using SP_ManageProduct. Reason: {Reason}", product.Product_Name, failureReason);
                    return null; 
                }

                if (newProductId.HasValue && product.Images != null && product.Images.Any())
                {
                    _logger?.LogInformation("Adding {ImageCount} images for product ID: {ProductId}", product.Images.Count, newProductId.Value);
                    foreach (var productImage in product.Images)
                    {
                        var imageParameters = new DynamicParameters();
                        imageParameters.Add("@Action", "AddProductImage");
                        imageParameters.Add("@ProductID", newProductId.Value); // Use the newly obtained ProductID
                        imageParameters.Add("@ImagePath", productImage.Image_Path);
                        imageParameters.Add("@IsPrimary", productImage.IsPrimary);
                        imageParameters.Add("@SortOrder", productImage.SortOrder);
                        // Add other parameters for AddProductImage if any (e.g., @ImageID for update, but not for add)

                        try
                        {
                            var imageResult = await _dbConnection.QuerySingleOrDefaultAsync<dynamic>(
                                "SP_ManageProduct",
                                imageParameters,
                                commandType: CommandType.StoredProcedure
                            );

                            if (imageResult == null)
                            {
                                _logger?.LogWarning("SP_ManageProduct returned null result when adding image {ImagePath} for product ID {ProductId}.",
                                    productImage.Image_Path, newProductId.Value);
                                // Decide if this is a critical failure. For now, we'll log and continue with other images.
                                continue;
                            }

                            var imageResultDict = (IDictionary<string, object>)imageResult;
                            bool imageAddedSuccessfully = false;
                            if (imageResultDict.TryGetValue("Success", out object? imgSuccessValue))
                            {
                                if ((imgSuccessValue is bool boolVal && boolVal) || (imgSuccessValue is int intVal && intVal == 1))
                                {
                                    imageAddedSuccessfully = true;
                                }
                            }

                            if (imageAddedSuccessfully)
                            {
                                int? newImageId = null;
                                if (imageResultDict.TryGetValue("ImageID", out object? imgIdVal) && imgIdVal is int imgId)
                                {
                                    newImageId = imgId;
                                }
                                _logger?.LogInformation("Successfully added image {ImagePath} (ID: {ImageID}) for product ID: {ProductId}",
                                    productImage.Image_Path, newImageId.HasValue ? newImageId.Value.ToString() : "N/A", newProductId.Value);
                            }
                            else
                            {
                                string imageFailureReason = "Unknown error adding product image.";
                                if (imageResultDict.TryGetValue("Result", out object? reasonValue) && reasonValue != null)
                                {
                                    imageFailureReason = reasonValue.ToString() ?? imageFailureReason;
                                }
                                _logger?.LogWarning("Failed to add image {ImagePath} for product ID {ProductId}. Reason: {Reason}",
                                    productImage.Image_Path, newProductId.Value, imageFailureReason);
                            }
                        }
                        catch (Exception imgEx)
                        {
                            _logger?.LogError(imgEx, "Exception occurred while adding image {ImagePath} for product ID {ProductId}",
                                productImage.Image_Path, newProductId.Value);
                        }
                    }
                }
                else if (newProductId.HasValue)
                {
                    _logger?.LogInformation("No additional images to add for product ID: {ProductId}", newProductId.Value);
                }

                return newProductId.Value; 
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception occurred in AddProductAsync for Product {ProductName}", product.Product_Name);
                return null; 
            }
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "UpdateProduct");
            parameters.Add("@ProductID", product.Product_ID);
            parameters.Add("@ProductName", product.Product_Name);
            parameters.Add("@ProductIcon", product.Product_Icon);
            parameters.Add("@ProductCode", product.Product_Code);
            parameters.Add("@MasterCategoryID", product.MasterCategory_ID);
            parameters.Add("@MasterSubCategoryID", product.MasterSubCategory_ID);
            parameters.Add("@SubCategoryID", product.SubCategory_ID);
            parameters.Add("@ShortDescription", product.Short_Description);
            parameters.Add("@LongDescription", product.Long_Description);
            parameters.Add("@Price", product.Price);
            parameters.Add("@MRP", product.MRP);
            parameters.Add("@DiscountPercent", product.Discount_Percent);
            parameters.Add("@StockQuantity", product.Stock_Quantity);
            parameters.Add("@IsFeatured", product.IsFeatured);
            parameters.Add("@IsAvailable", product.IsAvailable);

            try
            {
                var result = await _dbConnection.QuerySingleOrDefaultAsync<dynamic>(
                    "SP_ManageProduct",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    _logger?.LogWarning("SP_ManageProduct returned null result when updating product {ProductId}.", product.Product_ID);
                    return false;
                }

                var resultDict = (IDictionary<string, object>)result;
                bool success = false;

                if (resultDict.TryGetValue("Success", out object? successValue))
                {
                    if (successValue is bool boolVal && boolVal) success = true;
                    else if (successValue is int intVal && intVal == 1) success = true;
                }

                if (!success)
                {
                    string failureReason = "Stored procedure indicated failure.";
                    if (resultDict.TryGetValue("Result", out object? reasonValue) && reasonValue != null)
                    {
                        failureReason = reasonValue.ToString() ?? failureReason;
                    }
                    _logger?.LogWarning("Failed to update product {ProductId} using SP_ManageProduct. Reason: {Reason}", product.Product_ID, failureReason);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception occurred in UpdateProductAsync for Product {ProductId}", product.Product_ID);
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var result = await _dbConnection.QuerySingleOrDefaultAsync<dynamic>(
                   "SP_ManageProduct",
                   new { Action = "DeleteProduct", ProductID = productId },
                   commandType: CommandType.StoredProcedure
               );

                if (result == null)
                {
                    _logger?.LogWarning("SP_ManageProduct returned null result when deleting product {ProductId}.", productId);
                    return false;
                }

                var resultDict = (IDictionary<string, object>)result;
                bool success = false;

                if (resultDict.TryGetValue("Success", out object? successValue))
                {
                    if (successValue is bool boolVal && boolVal) success = true;
                    else if (successValue is int intVal && intVal == 1) success = true;
                }

                if (!success)
                {
                    string failureReason = "Stored procedure indicated failure.";
                    if (resultDict.TryGetValue("Result", out object? reasonValue) && reasonValue != null)
                    {
                        failureReason = reasonValue.ToString() ?? failureReason;
                    }
                    _logger?.LogWarning("Failed to delete product {ProductId} using SP_ManageProduct. Reason: {Reason}", productId, failureReason);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception occurred in DeleteProductAsync for Product {ProductId}", productId);
                return false;
            }
        }

        
    }
}

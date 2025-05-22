using DapperAPI.DTOs;
using DapperAPi.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core_Api_Using_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products.");
                return StatusCode(500, "Internal server error retrieving products.");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product {ProductId}.", id);
                return StatusCode(500, $"Internal server error retrieving product {id}.");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                int? newProductId = await _productRepository.AddProductAsync(product);

                if (newProductId.HasValue)
                {
                    return CreatedAtAction(nameof(GetProduct), new { id = newProductId.Value }, new { ProductId = newProductId.Value, Message = "Product created successfully." });
                }
                else
                {
                    _logger.LogWarning("Failed to add product {ProductName}.", product.Product_Name);
                    return BadRequest(new { Message = "Failed to add product. Check server logs for details." });
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Exception adding product {ProductName}", product.Product_Name);
                return StatusCode(500, new { Message = "An unexpected server error occurred while adding the product." });
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Product_ID) return BadRequest("Product ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                bool success = await _productRepository.UpdateProductAsync(product);
                if (success)
                {
                    return NoContent(); 
                }
                else
                {
                   
                    _logger.LogWarning("Failed to update product {ProductId}. It might not exist or an error occurred.", id);
                    var exists = await _productRepository.GetProductByIdAsync(id);
                    if (exists == null)
                    {
                        return NotFound(new { Message = $"Product with ID {id} not found." });
                    }
                    return BadRequest(new { Message = "Failed to update product. Check server logs." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception updating product {ProductId}", id);
                return StatusCode(500, new { Message = "An unexpected server error occurred while updating the product." });
            }
        }


        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool success = await _productRepository.DeleteProductAsync(id);
                if (success)
                {
                    return NoContent(); 
                }
                else
                {
                    _logger.LogWarning("Failed to delete product {ProductId}. It might not exist or an error occurred.", id);
                    
                    var exists = await _productRepository.GetProductByIdAsync(id);
                    if (exists == null)
                    {
                       
                        return NotFound(new { Message = $"Product with ID {id} not found or already deleted." });
                    }
                    return BadRequest(new { Message = "Failed to delete product. Check server logs." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception deleting product {ProductId}", id);
                return StatusCode(500, new { Message = "An unexpected server error occurred while deleting the product." });
            }
        }
    }
}

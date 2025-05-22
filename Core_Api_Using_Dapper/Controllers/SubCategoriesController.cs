using DapperAPI.DTOs;
using DapperAPi.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core_Api_Using_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ILogger<SubCategoriesController> _logger;
    

        public SubCategoriesController(ISubCategoryRepository subCategoryRepository, ILogger<SubCategoriesController> logger /*, IMasterSubCategoryRepository masterSubCategoryRepository*/)
        {
            _subCategoryRepository = subCategoryRepository;
            _logger = logger;
        }

        // GET: api/subcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategories()
        {
            try
            {
                var subCategories = await _subCategoryRepository.GetSubCategoriesAsync();
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sub-categories.");
                return StatusCode(500, "Internal server error retrieving sub-categories.");
            }
        }

        // POST: api/subcategories
        [HttpPost]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategory subCategory)
        {
            if (!ModelState.IsValid || subCategory == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                await _subCategoryRepository.AddSubCategoryAsync(subCategory);
                return Ok(new { Message = "Sub-category added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding sub-category {SubCategoryName}", subCategory.SubCategory_Name);
                return StatusCode(500, "Internal server error adding sub-category.");
            }
        }

        // PUT: api/subcategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubCategory(int id, [FromBody] SubCategory subCategory)
        {
            if (id != subCategory.SubCategory_ID)
            {
                return BadRequest("Sub-Category ID mismatch.");
            }

            if (!ModelState.IsValid || subCategory == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                await _subCategoryRepository.UpdateSubCategoryAsync(subCategory);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sub-category {SubCategoryId}", id);
                return StatusCode(500, "Internal server error updating sub-category.");
            }
        }

        // DELETE: api/subcategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Sub-Category ID.");
            }

            try
            {
                // Optional: Check if exists
                await _subCategoryRepository.DeleteSubCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sub-category {SubCategoryId}", id);
                return StatusCode(500, "Internal server error deleting sub-category.");
            }
        }
    }
}

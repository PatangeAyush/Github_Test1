using DapperAPI.DTOs;
using DapperAPi.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core_Api_Using_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterSubCategoriesController : ControllerBase
    {
        private readonly IMasterSubCategoryRepository _masterSubCategoryRepository;
        private readonly ILogger<MasterSubCategoriesController> _logger;

        public MasterSubCategoriesController(IMasterSubCategoryRepository masterSubCategoryRepository, ILogger<MasterSubCategoriesController> logger)
        {
            _masterSubCategoryRepository = masterSubCategoryRepository;
            _logger = logger;
        }

        // GET: api/mastersubcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterSubCategory>>> GetMasterSubCategories()
        {
            try
            {
                var masterSubCategories = await _masterSubCategoryRepository.GetMasterSubCategoriesAsync();
                return Ok(masterSubCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving master sub-categories.");
                return StatusCode(500, "Internal server error retrieving master sub-categories.");
            }
        }

        // POST: api/mastersubcategories
        [HttpPost]
        public async Task<IActionResult> AddMasterSubCategory([FromBody] MasterSubCategory masterSubCategory)
        {
            if (!ModelState.IsValid || masterSubCategory == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _masterSubCategoryRepository.AddMasterSubCategoryAsync(masterSubCategory);
                return Ok(new { Message = "Master sub-category added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding master sub-category {MasterSubCategoryName}", masterSubCategory.Master_SubCategory_Name);
                return StatusCode(500, "Internal server error adding master sub-category.");
            }
        }

        // PUT: api/mastersubcategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMasterSubCategory(int id, [FromBody] MasterSubCategory masterSubCategory)
        {
            if (id != masterSubCategory.Master_SubCategory_ID)
            {
                return BadRequest("Master Sub-Category ID mismatch.");
            }

            if (!ModelState.IsValid || masterSubCategory == null)
            {
                return BadRequest(ModelState);
            }

            try
            {

                await _masterSubCategoryRepository.UpdateMasterSubCategoryAsync(masterSubCategory);
                return NoContent();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error updating master sub-category {MasterSubCategoryId}", id);
                return StatusCode(500, "Internal server error updating master sub-category.");
            }
        }

        // DELETE: api/mastersubcategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterSubCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Master Sub-Category ID.");
            }

            try
            {
                await _masterSubCategoryRepository.DeleteMasterSubCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting master sub-category {MasterSubCategoryId}", id);
                return StatusCode(500, "Internal server error deleting master sub-category.");
            }
        }
    }
}

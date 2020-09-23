using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce_API.Abstractions;
using ECommerce_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo repo;

        public CategoriesController(ICategoryRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                return Ok(await repo.GetAll());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database. Error message:{e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            try
            {
                var result = await repo.GetById(id);

                if (result == null)
                {
                    return NotFound($"Category with Id = {id} not found");
                }

                return result;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database. Error message:{e.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            try
            {
                if (category != null)
                {
                    category.CategoryId = Guid.NewGuid().ToString();
                    var createdCategory = await repo.Create(category);
                    if (createdCategory != null)
                    {

                        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId },
                        createdCategory);

                    }
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database. Error message:{e.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, Category category)
        {
            try
            {
                var categoryToUpdate = await repo.GetById(id);

                if (categoryToUpdate == null)
                {
                    return NotFound($"Category with Id = {id} not found");
                }
                category.CategoryId = id;
                var result = await repo.Edit(category);
                if (result)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data. Error message:{e.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            try
            {
                var categoryToDelete = await repo.GetById(id);

                if (categoryToDelete == null)
                {
                    return NotFound($"Category with Id = {id} not found");
                }
                var result = await repo.Delete(id);
                if (result)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error Deleting data. Error message:{e.Message}");
            }
        }
    }
}
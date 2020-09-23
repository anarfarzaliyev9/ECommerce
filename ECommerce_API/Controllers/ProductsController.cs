using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ECommerce_API.Abstractions;
using ECommerce_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

namespace ECommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo repo;

        public ProductsController(IProductRepo repo)
        {
            this.repo = repo;
        }
       
        [HttpGet]
        public async Task<ActionResult> GetProducts()
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
        [Route("GetProductsWithCategory")]
        [HttpGet]
        public async Task<ActionResult> GetProductsWithCategory()
        {
            try
            {
                return Ok(await repo.GetAllProductWithCategory());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database. Error message:{e.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            try
            {
                var result = await repo.GetById(id);

                if (result == null)
                {
                    return NotFound($"Product with Id = {id} not found");
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
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try
            {
                if (product != null)
                {
                    product.Id = Guid.NewGuid().ToString();
                    var createdProduct = await repo.Create(product);
                    if (createdProduct != null)
                    {
                    
                    return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id },
                    createdProduct);

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
        public async Task<ActionResult> UpdateProduct(string id, Product product)
        {
            try
            {
                var productToUpdate = await repo.GetById(id);

                if (productToUpdate == null)
                {
                    return NotFound($"Product with Id = {id} not found");
                }
                product.Id = id;
                var result = await repo.Edit(product);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
                catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data. Error message:{e.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            try
            {
                var productToDelete = await repo.GetById(id);

                if (productToDelete == null)
                {
                    return NotFound($"Product with Id = {id} not found");
                }
                var result= await repo.Delete(id);
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
using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await repo.GetProductsAsync());
    }

    [HttpGet("id:int")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var Product = await repo.GetProductByIdAsync(id);

        if(Product == null)
            return NotFound();

        return Product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProducts",new {id=product.Id},product);
        }

        return BadRequest("Problem while creating a Product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if(product.Id != id || !ProductExists(id))
            return BadRequest("Cannot Update this");

        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Error While Updating Product Info");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product =  await repo.GetProductByIdAsync(id);

        if(product == null)
            return NotFound();

        repo.DeleteProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Error While Deleting Product");
    }

    private  bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}

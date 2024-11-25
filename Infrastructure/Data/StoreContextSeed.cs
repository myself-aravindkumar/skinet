using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if(!context.Products.Any())
        {
            var ProductsData =  await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/Products.json");

            var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

            if(Products != null)
            {
                context.Products.AddRange(Products);
                await context.SaveChangesAsync();
            }
        }
    }
}

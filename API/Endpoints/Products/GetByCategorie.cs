using Application.Controllers.Products;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Product.Output;
using Shared.Result;

namespace API.Endpoints.Products;
internal sealed class GetByCategorie : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products",
           async (AppDbContext appDbContext, HttpContext httpContext, [FromQuery] string? idCategorie = null, [FromQuery] string? nameCategorie = null) =>
           {
               IProductDataSource dataSource = new ProductDataSource(appDbContext);
               ProductController _productController = new ProductController(dataSource);
               var products = await _productController.GetProductsByCategorie(idCategorie, nameCategorie);

               return products.Succeeded ? Results.Ok(products) : Results.BadRequest(products);

           })
           .WithTags("Products")
           .Produces<ICommandResult<List<ProductOutputDto>>>()
           .WithName("Product.GetByCategorie").RequireAuthorization();//.RequireAuthorization();

    }
}



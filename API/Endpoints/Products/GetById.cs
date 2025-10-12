using Application.Controllers.Products;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Product.Output;
using Shared.Result;

namespace API.Endpoints.Products;
internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/{id}",
           async (AppDbContext appDbContext, [FromRoute] Guid id) =>
           {
               IProductDataSource dataSource = new ProductDataSource(appDbContext);
               ProductController _productController = new ProductController(dataSource);
               var categorie = await _productController.GetProductById(id);

               return categorie.Succeeded ? Results.Ok(categorie) : Results.NotFound(categorie);

           })
           .WithTags("Products")
           .Produces<ICommandResult<ProductOutputDto?>>()
           .WithName("Product.GetById").RequireAuthorization();//.RequireAuthorization();

    }
}

using Application.Controllers.Products;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Endpoints.Products;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/products/{id}",
           async (AppDbContext appDbContext, FileStorageSettings _settings, [FromRoute] Guid id) =>
           {
               IFileStorageService _fileStorage = new FileStorageService(_settings);

               IProductDataSource dataSource = new ProductDataSource(appDbContext);
               ProductController _productController = new ProductController(dataSource,null,null, _fileStorage);
               var product = await _productController.DeleteProduct(id);

               return product.Succeeded ? Results.NoContent() : Results.BadRequest(product);

           })
           .WithTags("Products")
           .Produces<ICommandResult>()
           .WithName("Product.Delete").RequireAuthorization(); //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Master" }); //Comentado porque para a fase 3 não terá essa feature

    }
}

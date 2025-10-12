using Application.Controllers.Products;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Product.Output;
using Shared.DTO.Product.Request;
using Shared.Result;

namespace API.Endpoints.Products;
internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{id}",
           async (AppDbContext appDbContext, FileStorageSettings _settings, [FromRoute] Guid id, [FromBody] ProductRequestDto productDto) =>
           {
               if (!MiniValidator.TryValidate(productDto, out var errors))
                   return Results.ValidationProblem(errors);

               IProductDataSource dataSource = new ProductDataSource(appDbContext);
               ICategorieDataSource dataSourceCategorie = new CategorieDataSource(appDbContext);
               IIngredientDataSource dataSourceIngrediente = new IngredientDataSource(appDbContext);
               IFileStorageService _fileStorage = new FileStorageService(_settings);

               ProductController _productController = new ProductController(dataSource, dataSourceIngrediente, dataSourceCategorie, _fileStorage);
               var categorie = await _productController.UpdateProduct(productDto, id);

               return categorie.Succeeded ? Results.Ok(categorie) : Results.BadRequest(categorie);

           })
           .WithTags("Products")
           .Produces<ICommandResult<ProductOutputDto?>>()
           .WithName("Product.Update").RequireAuthorization()
           ; //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Master" }); //Comentado porque para a fase 3 não terá essa feature
    }
}

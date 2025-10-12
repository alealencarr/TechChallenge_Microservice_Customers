using Application.Controllers.Categories;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Categorie.Output;
using Shared.Result;

namespace API.Endpoints.Categories
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/categories/{id}",
               async (AppDbContext appDbContext, [FromRoute] Guid id) =>
               {
                   ICategorieDataSource dataSource = new CategorieDataSource(appDbContext);
                   CategorieController _categorieController = new CategorieController(dataSource);
                   var categorie = await _categorieController.GetCategorieByIdAsync(id);

                   return categorie.Succeeded ? Results.Ok(categorie) : Results.NotFound(categorie);

               })
               .WithTags("Categories")
               .Produces<ICommandResult<CategorieOutputDto?>>()
               .WithName("Categorie.GetById").RequireAuthorization(); //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Master" }); //Comentado porque para a fase 3 não terá essa feature

        }
    }
}



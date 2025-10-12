using Application.Controllers.Categories;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Categorie.Request;
using Shared.Result;

namespace API.Endpoints.Categories
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/categories/{id}",
               async (AppDbContext appDbContext, [FromRoute] Guid id, [FromBody] CategorieRequestDto categorieDto) =>
               {
                   ICategorieDataSource dataSource = new CategorieDataSource(appDbContext);
                   CategorieController _categorieController = new CategorieController(dataSource);
                   var categorie = await _categorieController.UpdateCategorie(categorieDto, id);

                   return categorie.Succeeded ? Results.Ok(categorie) : Results.BadRequest(categorie);

               })
               .WithTags("Categories")
               .Produces<ICommandResult<CategorieOutputDto?>>()
               .WithName("Categorie.Update").RequireAuthorization(); //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Master" }); //Comentado porque para a fase 3 não terá essa feature

        }
    }
}

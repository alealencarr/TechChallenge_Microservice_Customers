using Application.Controllers.Categories;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Endpoints.Categories
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/categories/{id}",
               async (AppDbContext appDbContext, [FromRoute] Guid id) =>
               {
                   ICategorieDataSource dataSource = new CategorieDataSource(appDbContext);
                   CategorieController _categorieController = new CategorieController(dataSource);
                   var categorie = await _categorieController.DeleteCategorie(id);

                   return categorie.Succeeded ? Results.NoContent() : Results.BadRequest(categorie);

               })
               .WithTags("Categories")
               .Produces<ICommandResult>()
               .WithName("Categorie.Delete").RequireAuthorization()
               ; //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Master" }); //Comentado porque para a fase 3 não terá essa feature
        }
    }
}

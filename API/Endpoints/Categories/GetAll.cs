using Application.Controllers.Categories;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Shared.DTO.Categorie.Output;
using Shared.Result;

namespace API.Endpoints.Categories
{
    internal sealed class GetAll : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/categories",
               async (AppDbContext appDbContext, HttpContext httpContext) =>
               {
                   ICategorieDataSource dataSource = new CategorieDataSource(appDbContext);
                   CategorieController _categorieController = new CategorieController(dataSource);
                   var categories = await _categorieController.GetAllCategoriesAsync();

                   return categories.Succeeded ? Results.Ok(categories) : Results.BadRequest(categories);

               })
               .WithTags("Categories")
               .Produces<ICommandResult<List<CategorieOutputDto>>>()
               .WithName("Categorie.GetAll").RequireAuthorization(); //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Master" }); //Comentado porque para a fase 3 não terá essa feature

        }
    }
}



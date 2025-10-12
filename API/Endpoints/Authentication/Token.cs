//using Application.Controllers.Authentication;
//using Application.Interfaces.DataSources;
//using Application.Interfaces.Services;
//using Infrastructure.DataSources;
//using Infrastructure.DbContexts;
//using Microsoft.AspNetCore.Mvc;
//using MiniValidation;
//using Shared.DTO.Authentication.Output;
//using Shared.DTO.Authentication.Request;
//using Shared.Result;

//namespace API.Endpoints.Authentication;
//internal sealed class Token : IEndpoint
//{
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapPost("api/authentication/token",
//           async (AppDbContext appDbContext, ITokenService tokenService, IPasswordService passwordService, [FromBody] AuthenticationLoginRequestDto authenticationDto) =>
//           {
//               if (!MiniValidator.TryValidate(authenticationDto, out var errors))
//                   return Results.ValidationProblem(errors);

//               IUserDataSource dataSource = new UserDataSource(appDbContext);
//               AuthenticationController _userController = new(dataSource, tokenService, passwordService);
//               var user = await _userController.Authentication(authenticationDto);

//               return user.Succeeded ? Results.Ok(user) : Results.BadRequest(user);

//           })
//           .WithTags("Authentication")
//           .Produces<ICommandResult<TokenDto?>>()
//           .WithName("Authentication.Token");
//    }
//}

////Comentado porque a auth está na Function do Azure agora

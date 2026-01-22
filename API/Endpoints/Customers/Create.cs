using Application.Controllers.Customers;
using Application.Interfaces.DataSources;
using Domain.Entities;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Customer.Request;
using Shared.Result;
using System.Diagnostics.CodeAnalysis;

namespace API.Endpoints.Customers
{
    [ExcludeFromCodeCoverage]
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/customers",
               async (AppDbContext appDbContext, [FromBody] CustomerRequestDto customerDto) =>
               {
                   if (!MiniValidator.TryValidate(customerDto, out var errors))
                       return Results.ValidationProblem(errors);

                   ICustomerDataSource dataSource = new CustomerDataSource(appDbContext);
                   CustomerController _customerController = new CustomerController(dataSource);
                   var customer = await _customerController.CreateCustomer(customerDto);

                   return customer.Conflict ? Results.Conflict(customer) : customer.Succeeded ? Results.Created($"/{customer.Data?.Id}", customer) : Results.BadRequest(customer);

               })
               .WithTags("Customers")
               .Produces<ICommandResult<CustomerOutputDto?>>()
               .WithName("Customer.Create")
               .RequireAuthorization();
        }
    }
}

 
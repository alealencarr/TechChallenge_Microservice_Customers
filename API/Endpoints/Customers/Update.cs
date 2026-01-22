 using Application.Controllers.Customers;
using Application.Interfaces.DataSources;
using Domain.Entities;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Customer.Request;
using Shared.Result;
using System.Diagnostics.CodeAnalysis;

namespace API.Endpoints.Customers
{
    [ExcludeFromCodeCoverage]

    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/customers/{id}",
               async (AppDbContext appDbContext, [FromRoute] Guid id, [FromBody] CustomerRequestDto customerDto) =>
               {
                   ICustomerDataSource dataSource = new CustomerDataSource(appDbContext);
                   CustomerController _customerController = new CustomerController(dataSource);
                   var customer = await _customerController.UpdateCustomer(customerDto, id);

                   return customer.Succeeded ? Results.Ok(customer) : Results.BadRequest(customer);

               })
               .WithTags("Customers")
               .Produces<ICommandResult<CustomerOutputDto?>>()
               .WithName("Customer.Update").RequireAuthorization();
               //.RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Master" });
        }
    }
}

 
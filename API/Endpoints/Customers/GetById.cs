using Application.Controllers.Customers;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Categorie.Output;
using Shared.Result;
using System.Diagnostics.CodeAnalysis;

namespace API.Endpoints.Customers
{
    [ExcludeFromCodeCoverage]

    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/customers/id/{id}",
               async (AppDbContext appDbContext, [FromRoute] Guid id) =>
               {
                   ICustomerDataSource dataSource = new CustomerDataSource(appDbContext);
                   CustomerController _customerController = new CustomerController(dataSource);
                   var customer = await _customerController.GetCustomerById(id);

                   return customer.Succeeded ? Results.Ok(customer) : Results.NotFound(customer);

               })
               .WithTags("Customers")
               .Produces<ICommandResult<CustomerOutputDto?>>()
               .WithName("Customer.GetById").RequireAuthorization();
            //;//.RequireAuthorization();
        }
    }
}

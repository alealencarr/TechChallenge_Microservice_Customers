using Application.Controllers.Customers;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Categorie.Output;
using Shared.Result;

namespace API.Endpoints.Customers
{
    internal sealed class GetByCpf : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/customers/{cpf}",
               async (AppDbContext appDbContext, [FromRoute] string cpf) =>
               {
                   ICustomerDataSource dataSource = new CustomerDataSource(appDbContext);
                   CustomerController _customerController = new CustomerController(dataSource);
                   var customer = await _customerController.GetCustomerByCpf(cpf);

                   return customer.Succeeded ? Results.Ok(customer) : Results.NotFound(customer);

               })
               .WithTags("Customers")
               .Produces<ICommandResult<CustomerOutputDto?>>()
               .WithName("Customer.GetByCpf").RequireAuthorization();
            //;//.RequireAuthorization();
        }
    }
}

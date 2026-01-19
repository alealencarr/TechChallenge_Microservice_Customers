using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Customers
{
    public class GetCustomerByIdUseCase
    {
        CustomerGateway _gateway = null;
        public static GetCustomerByIdUseCase Create(CustomerGateway gateway)
        {
            return new GetCustomerByIdUseCase(gateway);
        }

        private GetCustomerByIdUseCase(CustomerGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Customer?> Run(Guid id)
        {
            try
            {
                var customer = await _gateway.GetById(id);

                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}

using Application.Common;
using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Presenter.Categories;
using Application.UseCases.Customers;
using Application.UseCases.Customers.Command;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Customer.Request;
using Shared.Result;

namespace Application.Controllers.Customers
{
    public class CustomerController
    {
        private ICustomerDataSource _dataSource;
        public CustomerController(ICustomerDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<ICommandResult<CustomerOutputDto>> CreateCustomer(CustomerRequestDto customerRequestDto)
        {
            CustomerPresenter customerPresenter = new("Cliente cadastrado!");

            try
            {
                var command = new CustomerCommand(customerRequestDto.Cpf.FormataCpfSemPontuacao(), customerRequestDto.Name, customerRequestDto.Mail);

                var customerGateway = CustomerGateway.Create(_dataSource);
                var useCaseCreate =  CreateCustomerUseCase.Create(customerGateway);
                var customerEntity = await useCaseCreate.Run(command);

                if (!customerEntity.Item2)
                    return customerPresenter.TransformObject(customerEntity.Item1);              
                
                return customerPresenter.Conflict<CustomerOutputDto>("Cliente com este CPF já está cadastrado.");
            }
            catch (Exception ex)
            {
                return customerPresenter.Error<CustomerOutputDto>(ex.Message);
            }
        }

        public async Task<ICommandResult<CustomerOutputDto>> UpdateCustomer(CustomerRequestDto customerRequestDto, Guid id)
        {
            CustomerPresenter customerPresenter = new("Cliente alterado!");

            try
            {
                var command = new CustomerCommand(id, customerRequestDto.Cpf.FormataCpfSemPontuacao(), customerRequestDto.Name, customerRequestDto.Mail);

                var customerGateway = CustomerGateway.Create(_dataSource);
                var useCaseCreate = UpdateCustomerUseCase.Create(customerGateway);
                var customerEntity = await useCaseCreate.Run(command);

                var dtoRetorno = customerPresenter.TransformObject(customerEntity);

                return dtoRetorno;
            }
            catch (Exception ex)
            {
                return customerPresenter.Error<CustomerOutputDto>(ex.Message);
            }
        }

        public async Task<ICommandResult<CustomerOutputDto?>> GetCustomerByCpf(string cpf)
        {
            CustomerPresenter customerPresenter = new("Cliente encontrado!");

            try
            {
                var customerGateway = CustomerGateway.Create(_dataSource);
                var useCase = GetCustomerByCpfUseCase.Create(customerGateway);
                var customer = await useCase.Run(cpf);

                return customer is null ? customerPresenter.Error<CustomerOutputDto?>("Customer not found.") : customerPresenter.TransformObject(customer);
            }
            catch (Exception ex)
            {
                return customerPresenter.Error<CustomerOutputDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult<CustomerOutputDto?>> GetCustomerById(Guid id)
        {
            CustomerPresenter customerPresenter = new("Cliente encontrado!");

            try
            {
                var customerGateway = CustomerGateway.Create(_dataSource);
                var useCase = GetCustomerByIdUseCase.Create(customerGateway);
                var customer = await useCase.Run(id);

                return customer is null ? customerPresenter.Error<CustomerOutputDto?>("Customer not found.") : customerPresenter.TransformObject(customer);
            }
            catch (Exception ex)
            {
                return customerPresenter.Error<CustomerOutputDto?>(ex.Message);
            }
        }
    }
}

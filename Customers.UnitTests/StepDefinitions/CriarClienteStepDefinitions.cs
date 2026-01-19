using Application.Gateways;
using Application.UseCases.Customers;
using Application.UseCases.Customers.Command; 
using Domain.Entities;
using Application.Interfaces.DataSources; 
using FluentAssertions;
using Moq;
using Reqnroll;
using Shared.DTO.Categorie.Input;

namespace Customers.UnitTests.StepDefinitions
{
    [Binding]
    public class CustomerCreationSteps
    {
        private readonly Mock<ICustomerDataSource> _dataSourceMock;
        private readonly CustomerGateway _gateway;
        private readonly CreateCustomerUseCase _useCase;

        private CustomerCommand _commandInput;
        private (Customer Customer, bool Exists) _resultado;
        private Exception _exceptionResult;

        public CustomerCreationSteps()
        {
            _dataSourceMock = new Mock<ICustomerDataSource>();

            _gateway = CustomerGateway.Create(_dataSourceMock.Object);
            _useCase = CreateCustomerUseCase.Create(_gateway);
        }


        [Given("que eu tenho um cliente novo com CPF {string} nome {string} e email {string}")]
        public void GivenClienteNovo(string cpf, string nome, string email)
        {
            _commandInput = new CustomerCommand (id:Guid.NewGuid(), cpf: cpf, name: nome, mail: email );
        }

        [Given("que este CPF nao existe na base de dados")]
        public void GivenCpfNaoExiste()
        {
            _dataSourceMock.Setup(x => x.GetByCpf(_commandInput.Cpf))
                           .ReturnsAsync((CustomerInputDto?)null);
        }

        [Given("que eu informo um CPF {string} que ja esta cadastrado")]
        public void GivenCpfJaExiste(string cpf)
        {
            _commandInput = new CustomerCommand(id: Guid.NewGuid(), cpf: cpf, name: "Ja Existe", mail: "old@mail.com");


            var clienteExistente = new CustomerInputDto( Guid.NewGuid(), DateTime.Now, cpf, "Nome Original", "original@mail.com", true);


            _dataSourceMock.Setup(x => x.GetByCpf(cpf))
                           .ReturnsAsync(clienteExistente);
        }

        [Given("que eu tento cadastrar um cliente com email invalido")]
        public void GivenEmailInvalido()
        {


            _commandInput = new CustomerCommand(id: Guid.NewGuid(), cpf: "12345678900", name: "Teste", mail: "email_ruim");


            _dataSourceMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
                           .ReturnsAsync((CustomerInputDto?)null);
        }

 
        [When("eu solicito o cadastro deste cliente")]
        public async Task WhenSolicitoCadastro()
        {
            try
            {
                _resultado = await _useCase.Run(_commandInput);
            }
            catch (Exception ex)
            {
                _exceptionResult = ex;
            }
        }

        [Then("o cliente deve ser salvo no repositorio")]
        public void ThenDeveSalvar()
        {
       
            _exceptionResult.Should().BeNull("não deve haver erro de validação ao criar um cliente válido");

            _dataSourceMock.Verify(x => x.Create(It.IsAny<CustomerInputDto>()), Times.Once);
        }
         

        [Then("o sistema deve informar que foi um novo cadastro")]
        public void ThenNovoCadastro()
        {
            _resultado.Customer.Should().NotBeNull();
            _resultado.Exists.Should().BeFalse(); 
        }

        [Then("o sistema deve retornar o cliente existente")]
        public void ThenRetornaExistente()
        {
            _resultado.Customer.Should().NotBeNull();
            _resultado.Customer.Name.Should().Be("Nome Original"); 
        }

        [Then("o sistema deve informar que o cliente ja existia")]
        public void ThenInformaExistencia()
        {
            _resultado.Exists.Should().BeTrue();  
        }

        [Then("nao deve tentar salvar duplicado")]
        public void ThenNaoSalvaDuplicado()
        {
            _dataSourceMock.Verify(x => x.Create(It.IsAny<CustomerInputDto>()), Times.Never);
        }

        [Then("o sistema deve retornar um erro de validacao")]
        public void ThenErroValidacao()
        {
            _exceptionResult.Should().NotBeNull();
            _exceptionResult.Should().BeOfType<ArgumentException>();
        }
    }
}
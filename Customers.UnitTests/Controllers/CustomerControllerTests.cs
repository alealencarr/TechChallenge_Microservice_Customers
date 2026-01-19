using Application.Controllers.Customers;
using Application.Interfaces.DataSources;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Customer.Request;

namespace Customers.UnitTests.Application.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerDataSource> _dataSourceMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _dataSourceMock = new Mock<ICustomerDataSource>();
            _controller = new CustomerController(_dataSourceMock.Object);
        }


        [Fact]
        public async Task CreateCustomer_DeveRetornarSucesso_QuandoCpfNaoExiste()
        {

            var request = new CustomerRequestDto { Cpf = "71667731033", Name = "Novo", Mail = "new@mail.com" };


            _dataSourceMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
                           .ReturnsAsync((CustomerInputDto?)null);


            _dataSourceMock.Setup(x => x.Create(It.IsAny<CustomerInputDto>()))
                           .Returns(Task.CompletedTask);


            var result = await _controller.CreateCustomer(request);

            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Cliente cadastrado!");
            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task CreateCustomer_DeveRetornarConflito_QuandoCpfJaExiste()
        {
            var request = new CustomerRequestDto { Cpf = "30577867075", Name = "Duplicado", Mail = "dup@mail.com" };

            var existingUser = new CustomerInputDto(Guid.NewGuid(), DateTime.Now, "30577867075", "Existente", "old@mail.com", true);

            _dataSourceMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
                           .ReturnsAsync(existingUser);

            var result = await _controller.CreateCustomer(request);


            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Cliente com este CPF já está cadastrado.");

        }

        [Fact]
        public async Task CreateCustomer_DeveRetornarErro_QuandoExcecaoOcorre()
        {
            var request = new CustomerRequestDto { Cpf = "71667731033", Name = "Erro", Mail = "err@mail.com" };

            _dataSourceMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
                           .ThrowsAsync(new Exception("Erro de conexão"));

            var result = await _controller.CreateCustomer(request);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Error:Erro de conexão");

        }


        [Fact]
        public async Task UpdateCustomer_DeveRetornarSucesso_QuandoClienteExiste()
        {

            var id = Guid.NewGuid();
            var request = new CustomerRequestDto { Cpf = "71667731033", Name = "Atualizado", Mail = "upd@mail.com" };


            var existingUser = new CustomerInputDto(id, DateTime.Now, "30577867075", "Antigo", "old@mail.com", true);

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(existingUser);

            _dataSourceMock.Setup(x => x.Update(It.IsAny<CustomerInputDto>()))
                           .Returns(Task.CompletedTask);

            var result = await _controller.UpdateCustomer(request, id);


            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Cliente alterado!");
            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be("Atualizado");
        }

        [Fact]
        public async Task UpdateCustomer_DeveRetornarErro_QuandoClienteNaoExiste()
        {

            var id = Guid.NewGuid();
            var request = new CustomerRequestDto { Cpf = "30577867075", Name = "Atualizado", Mail = "upd@mail.com" };


            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync((CustomerInputDto?)null);

            var result = await _controller.UpdateCustomer(request, id);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Error:Error: Customer not find by Id.");


        }



        [Fact]
        public async Task GetCustomerByCpf_DeveRetornarSucesso_QuandoEncontrado()
        {
            var cpf = "30577867075";
            var existingUser = new CustomerInputDto(Guid.NewGuid(), DateTime.Now, cpf, "Teste", "teste@mail.com", true);

            _dataSourceMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
                           .ReturnsAsync(existingUser);

            var result = await _controller.GetCustomerByCpf(cpf);



            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Cliente encontrado!");
            result.Data.Should().NotBeNull();
            result.Data.Cpf.Should().Be(cpf);
        }

        [Fact]
        public async Task GetCustomerByCpf_DeveRetornarErro_QuandoNaoEncontrado()
        {

            var cpf = "00000000000";
            _dataSourceMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
                           .ReturnsAsync((CustomerInputDto?)null);


            var result = await _controller.GetCustomerByCpf(cpf);

 
            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Customer not found.");
        }



        [Fact]
        public async Task GetCustomerById_DeveRetornarSucesso_QuandoEncontrado()
        {

            var id = Guid.NewGuid();
            var existingUser = new CustomerInputDto(id, DateTime.Now, "30577867075", "Teste", "teste@mail.com", true);

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(existingUser);


            var result = await _controller.GetCustomerById(id);

            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Cliente encontrado!");
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetCustomerById_DeveRetornarErro_QuandoNaoEncontrado()
        {

            var id = Guid.NewGuid();
            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync((CustomerInputDto?)null);


            var result = await _controller.GetCustomerById(id);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Customer not found.");
        }
    }
}
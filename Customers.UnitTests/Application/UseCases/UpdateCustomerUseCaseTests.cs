using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Customers;
using Application.UseCases.Customers.Command;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Xunit;

namespace Customers.UnitTests.Application.UseCases
{
    public class UpdateCustomerUseCaseTests
    {
        private readonly Mock<ICustomerDataSource> _dataSourceMock;
        private readonly CustomerGateway _gateway;
        private readonly UpdateCustomerUseCase _useCase;

        public UpdateCustomerUseCaseTests()
        {
            _dataSourceMock = new Mock<ICustomerDataSource>();
            _gateway = CustomerGateway.Create(_dataSourceMock.Object);
            _useCase = UpdateCustomerUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Run_DeveAtualizarCliente_QuandoDadosValidos()
        {
            var id = Guid.NewGuid();

            var cpfValido = "71667731033";

            var existingDto = new CustomerInputDto(id, DateTime.Now, "30577867075", "Nome Antigo", "old@mail.com", true);

            var command = new CustomerCommand(id, cpfValido, "Nome Novo", "new@mail.com");

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(existingDto);

            _dataSourceMock.Setup(x => x.Update(It.IsAny<CustomerInputDto>()))
                           .Returns(Task.CompletedTask);


            var result = await _useCase.Run(command);


            result.Should().NotBeNull();
            result.Name.Should().Be("Nome Novo");
            result.Mail.Should().Be("new@mail.com");
            result.Cpf.Valor.Should().Be(cpfValido);

            _dataSourceMock.Verify(x => x.Update(It.IsAny<CustomerInputDto>()), Times.Once);
        }

        [Fact]
        public async Task Run_DeveLancarException_QuandoClienteNaoExiste()
        {

            var id = Guid.NewGuid();
            var command = new CustomerCommand(id, "12345678900", "Nome", "email@mail.com");

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync((CustomerInputDto?)null);


            Func<Task> act = async () => await _useCase.Run(command);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Error:Error: Customer not find by Id.");

            _dataSourceMock.Verify(x => x.Update(It.IsAny<CustomerInputDto>()), Times.Never);
        }

        [Fact]
        public async Task Run_DeveLancarArgumentException_QuandoCpfInvalido()
        {
            var id = Guid.NewGuid();
            var cpfInvalido = "111222333"; 
            var command = new CustomerCommand(id, cpfInvalido, "Nome", "email@mail.com");

            var existingDto = new CustomerInputDto(id, DateTime.Now, "70293466034", "Nome Antigo", "old@mail.com", true);

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(existingDto);

            Func<Task> act = async () => await _useCase.Run(command);

            await act.Should().ThrowAsync<ArgumentException>();

            _dataSourceMock.Verify(x => x.Update(It.IsAny<CustomerInputDto>()), Times.Never);
        }

        [Fact]
        public async Task Run_DeveLancarExcecaoFormatada_QuandoErroGenerico()
        {

            var id = Guid.NewGuid();
            var command = new CustomerCommand(id, "70293466034", "Nome", "email@mail.com");

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ThrowsAsync(new Exception("Erro banco de dados"));


            Func<Task> act = async () => await _useCase.Run(command);


            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Error:Erro banco de dados");
        }
    }
}
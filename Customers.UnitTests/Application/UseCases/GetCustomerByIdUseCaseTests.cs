using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Customers;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Xunit;

namespace Customers.UnitTests.Application.UseCases
{
    public class GetCustomerByIdUseCaseTests
    {
        private readonly Mock<ICustomerDataSource> _dataSourceMock;
        private readonly CustomerGateway _gateway;
        private readonly GetCustomerByIdUseCase _useCase;

        public GetCustomerByIdUseCaseTests()
        {
            _dataSourceMock = new Mock<ICustomerDataSource>();
            _gateway = CustomerGateway.Create(_dataSourceMock.Object);
            _useCase = GetCustomerByIdUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Run_DeveRetornarCliente_QuandoIdExiste()
        {
            var id = Guid.NewGuid();
            var customerDto = new CustomerInputDto(id, DateTime.Now, "71667731033", "Teste ID", "teste@mail.com", true);

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(customerDto);

            var result = await _useCase.Run(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Teste ID");
        }

        [Fact]
        public async Task Run_DeveRetornarNulo_QuandoIdNaoExiste()
        {
            var id = Guid.NewGuid();
            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync((CustomerInputDto?)null);

            var result = await _useCase.Run(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Run_DeveLancarExcecaoFormatada_QuandoErroGenerico()
        {
            var id = Guid.NewGuid();
            _dataSourceMock.Setup(x => x.GetById(id))
                           .ThrowsAsync(new Exception("Falha de conexão"));

            Func<Task> act = async () => await _useCase.Run(id);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Error:Falha de conexão");
        }
    }
}
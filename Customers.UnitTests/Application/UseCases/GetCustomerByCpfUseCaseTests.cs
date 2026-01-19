using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Customers;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Xunit;

namespace Customers.UnitTests.Application.UseCases
{
    public class GetCustomerByCpfUseCaseTests
    {
        private readonly Mock<ICustomerDataSource> _dataSourceMock;
        private readonly CustomerGateway _gateway;
        private readonly GetCustomerByCpfUseCase _useCase;

        public GetCustomerByCpfUseCaseTests()
        {
            _dataSourceMock = new Mock<ICustomerDataSource>();
            _gateway = CustomerGateway.Create(_dataSourceMock.Object);
            _useCase = GetCustomerByCpfUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Run_DeveRetornarCliente_QuandoCpfExiste()
        {
            var cpf = "71667731033";
            var customerDto = new CustomerInputDto(Guid.NewGuid(), DateTime.Now, cpf, "Teste", "teste@mail.com", true);

            _dataSourceMock.Setup(x => x.GetByCpf(cpf))
                           .ReturnsAsync(customerDto);

            var result = await _useCase.Run(cpf);

            result.Should().NotBeNull();
            result!.Cpf!.Valor.Should().Be(cpf);
            result.Name.Should().Be("Teste");
            _dataSourceMock.Verify(x => x.GetByCpf(cpf), Times.Once);
        }

        [Fact]
        public async Task Run_DeveRetornarNulo_QuandoCpfNaoExiste()
        {
            var cpf = "00000000000";
            _dataSourceMock.Setup(x => x.GetByCpf(cpf))
                           .ReturnsAsync((CustomerInputDto?)null);

            var result = await _useCase.Run(cpf);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Run_DeveLancarExcecaoFormatada_QuandoOcorreErroNoGateway()
        {
            var cpf = "12345678900";
            _dataSourceMock.Setup(x => x.GetByCpf(cpf))
                           .ThrowsAsync(new Exception("Erro de banco"));

            Func<Task> act = async () => await _useCase.Run(cpf);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Error:Erro de banco");
        }
    }
}
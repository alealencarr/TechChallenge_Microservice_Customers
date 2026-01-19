using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Entities
{
    public class CustomerTests
    {
        private const string CpfValido = "05660825001";


        [Fact]
        public void Deve_Criar_Cliente_Valido()
        {
            var nome = "João da Silva";
            var email = "joao@email.com";

            var cliente = new Customer(CpfValido, nome, email);

            cliente.Should().NotBeNull();
            cliente.Id.Should().NotBeEmpty();
            cliente.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1)); 
            cliente.CustomerIdentified.Should().BeTrue();
            cliente.Name.Should().Be(nome);
            cliente.Mail.Should().Be(email);
            cliente.Cpf.Should().NotBeNull();
            cliente.Cpf!.Valor.Should().Be(CpfValido);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_ArgumentNullException_Quando_Cpf_Vazio_Ou_Nulo(string cpfInvalido)
        {
            Action act = () => new Customer(cpfInvalido, "Nome", "email@teste.com");

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'É necessário informar um Cpf para criar o cliente')");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_ArgumentNullException_Quando_Nome_Vazio_Ou_Nulo(string nomeInvalido)
        {
            Action act = () => new Customer(CpfValido, nomeInvalido, "email@teste.com");

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'É necessário informar um Nome para criar o cliente')");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_ArgumentNullException_Quando_Email_Vazio_Ou_Nulo(string emailInvalido)
        {
            Action act = () => new Customer(CpfValido, "Nome", emailInvalido);

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'É necessário informar um E-mail para criar o cliente')");
        }

        [Fact]
        public void Deve_Lancar_ArgumentException_Quando_Cpf_Formato_Invalido()
        {
            var cpfFormatoInvalido = "12345678900";

            Action act = () => new Customer(cpfFormatoInvalido, "Nome", "email@teste.com");
            act.Should().Throw<ArgumentException>()
               .WithMessage("Cpf inválido.");
        }


        [Fact]
        public void Deve_Reconstituir_Cliente_Completo()
        {
            var id = Guid.NewGuid();
            var dataCriacao = DateTime.Now.AddDays(-10);
            var nome = "Maria Reconstituida";
            var email = "maria@old.com";
            var identificado = false;

            var cliente = new Customer(id, dataCriacao, CpfValido, nome, email, identificado);

            cliente.Id.Should().Be(id);
            cliente.CreatedAt.Should().Be(dataCriacao);
            cliente.Name.Should().Be(nome);
            cliente.Mail.Should().Be(email);
            cliente.CustomerIdentified.Should().Be(identificado);
            cliente.Cpf!.Valor.Should().Be(CpfValido);
        }
    }
}
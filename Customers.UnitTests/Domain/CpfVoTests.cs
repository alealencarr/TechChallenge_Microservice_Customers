using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.ValueObjects
{
    public class CpfVoTests
    {
        [Fact]
        public void Deve_Criar_CpfVo_Com_Numero_Valido_Sem_Pontuacao()
        {
            var cpfValido = "30577867075";

            var cpfVo = new CpfVo(cpfValido);

            cpfVo.Should().NotBeNull();
            cpfVo.Valor.Should().Be(cpfValido);
        }

        [Fact]
        public void Deve_Criar_CpfVo_Com_Numero_Valido_Com_Pontuacao()
        {
            var cpfFormatado = "305.778.670-75";
            var cpfEsperado = "30577867075";

            var cpfVo = new CpfVo(cpfFormatado);

            cpfVo.Valor.Should().Be(cpfEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Deve_Lancar_ArgumentException_Quando_Cpf_Vazio_Ou_Nulo(string valorInvalido)
        {
            Action act = () => new CpfVo(valorInvalido);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Cpf precisa ser informado.");
        }

        [Theory]
        [InlineData("12345678900")] 
        [InlineData("11111111111")] 
        [InlineData("123")] 
        [InlineData("123456789012345")] 
        public void Deve_Lancar_ArgumentException_Quando_Cpf_Invalido(string cpfInvalido)
        {
            Action act = () => new CpfVo(cpfInvalido);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Cpf inválido.");
        }


        [Fact]
        public void ToString_Deve_Retornar_Cpf_Formatado()
        {
            var cpfLimpo = "30577867075";
            var cpfVo = new CpfVo(cpfLimpo);

            var resultado = cpfVo.ToString();

            resultado.Should().Be("305.778.670-75");
        }

        [Fact]
        public void Equals_Deve_Retornar_True_Para_Cpf_Com_Mesmo_Valor()
        {
            var cpf1 = new CpfVo("30577867075");
            var cpf2 = new CpfVo("305.778.670-75");

            cpf1.Equals(cpf2).Should().BeTrue();
        }

        [Fact]
        public void Equals_Deve_Retornar_False_Para_Cpfs_Diferentes()
        {
            var cpf1 = new CpfVo("30577867075");
            var cpf2 = new CpfVo("05660825001");

            cpf1.Equals(cpf2).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_Deve_Ser_Igual_Para_Mesmo_Valor()
        {
            var cpf1 = new CpfVo("30577867075");
            var cpf2 = new CpfVo("305.778.670-75");

            cpf1.GetHashCode().Should().Be(cpf2.GetHashCode());
        }
    }
}
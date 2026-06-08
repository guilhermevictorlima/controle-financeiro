namespace Data.Tests.Core.ValueObjects
{
    using Data.Core.Exceptions;
    using Data.Core.ValueObjects;

    public class CompetenciaTests
    {
        [Theory]
        [InlineData("2024-01")]
        [InlineData("2024-06")]
        [InlineData("2024-12")]
        [InlineData("1999-01")]
        [InlineData("2099-12")]
        public void Construtor_NaoDeveLancarExcecao_QuandoFormatoForValido(string anoMes)
        {
            Exception excecao = Record.Exception(() => new Competencia(anoMes));

            Assert.Null(excecao);
        }

        [Theory]
        [InlineData("2024-01")]
        [InlineData("2024-06")]
        [InlineData("2024-12")]
        public void Construtor_DeveAtribuirAnoMesCorretamente_QuandoFormatoForValido(string anoMes)
        {
            Competencia competencia = new(anoMes);

            Assert.Equal(anoMes, competencia.AnoMes);
        }

        [Theory]
        [InlineData("2024-00")]
        [InlineData("2024-13")]
        [InlineData("2024-1")]
        [InlineData("2024-001")]
        [InlineData("24-01")]
        [InlineData("2024/01")]
        [InlineData("2024.01")]
        [InlineData("01-2024")]
        [InlineData("2024")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd-ef")]
        [InlineData("2024-ab")]
        [InlineData("2024-6")]
        public void Construtor_DeveLancarExcecao_QuandoFormatoForInvalido(string anoMes)
        {
            FormatoCompetenciaException excecao = Assert.Throws<FormatoCompetenciaException>(
                () => new Competencia(anoMes));

            Assert.Equal("A competência deve estar no formato YYYY-MM.", excecao.Message);
        }
    }
}

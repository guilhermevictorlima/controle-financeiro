namespace Data.Tests.Models.Validator
{
    using Data.Models.Validators;
    using Xunit;

    public class CompetenciaValidatorTests
    {
        [Theory]
        [InlineData("2024-01")]
        [InlineData("2024-12")]
        [InlineData("2000-06")]
        [InlineData("1999-09")]
        public void Validate_QuandoCompetenciaValida_NaoLancaExcecao(string competencia)
        {
            var exception = Record.Exception(() => CompetenciaValidator.Validate(competencia));

            Assert.Null(exception);
        }

        [Theory]
        [InlineData("2024-00")]
        [InlineData("2024-13")]
        [InlineData("24-01")]
        [InlineData("2024/01")]
        [InlineData("2024-1")]
        [InlineData("")]
        [InlineData("teste")]
        [InlineData("abcd-01")]
        [InlineData("2024-ab")]
        public void Validate_QuandoCompetenciaInvalida_LancaArgumentException(string competencia)
        {
            var exception = Assert.Throws<ArgumentException>(() => CompetenciaValidator.Validate(competencia));

            Assert.Equal("A competência deve estar no formato YYYY-MM.", exception.Message);
        }
    }
}

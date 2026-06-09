namespace Business.Tests.Validators
{
    using Business.Exceptions;
    using Business.Validators;
    using Data.Core.ValueObjects;
    using Data.Models.Entities;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;
    using Moq;
    using Xunit;

    public class AlteracaoStatusLancamentoValidatorTests
    {
        private readonly Mock<ILancamentoFinanceiroRepository> repositoryMock;
        private readonly AlteracaoStatusLancamentoValidator validator;

        public AlteracaoStatusLancamentoValidatorTests()
        {
            this.repositoryMock = new Mock<ILancamentoFinanceiroRepository>();
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(CriarEntidade());

            this.validator = new AlteracaoStatusLancamentoValidator(this.repositoryMock.Object);
        }

        private static LancamentoFinanceiro CriarEntidade(StatusLancamento status = StatusLancamento.Aberto)
        {
            return new LancamentoFinanceiro
            {
                Id = 1,
                Descricao = "Pagamento de fornecedor",
                Tipo = TipoLancamento.Debito,
                ValorOriginal = 100.00m,
                PercentualTaxa = 0,
                PercentualDesconto = 0,
                ValorCalculado = 100.00m,
                DataLancamento = new DateTime(2024, 6, 15),
                DataCriacao = new DateTime(2024, 6, 15),
                DataPagamento = null,
                DataCancelamento = null,
                Competencia = new Competencia("2024-06"),
                Status = status,
            };
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidarCancelamento_DeveLancarExcecao_QuandoIdForInvalido(int id)
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarCancelamento(id));

            Assert.Equal("O identificador do lançamento é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void ValidarCancelamento_DeveLancarExcecao_QuandoLancamentoNaoForEncontrado()
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns((LancamentoFinanceiro?)null);

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarCancelamento(1));

            Assert.Equal("Lançamento com identificador 1 não encontrado.", excecao.Message);
        }

        [Theory]
        [InlineData(StatusLancamento.Pago)]
        [InlineData(StatusLancamento.Cancelado)]
        public void ValidarCancelamento_DeveLancarExcecao_QuandoStatusNaoForAberto(StatusLancamento status)
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(CriarEntidade(status));

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarCancelamento(1));

            Assert.Equal("Apenas lançamentos com status Aberto podem ser cancelados.", excecao.Message);
        }


        [Fact]
        public void ValidarCancelamento_NaoDeveLancarExcecao_QuandoLancamentoExisteEStatusForAberto()
        {
            Exception excecao = Record.Exception(() => this.validator.ValidarCancelamento(1));
            Assert.Null(excecao);
        }

        [Fact]
        public void ValidarCancelamento_NaoDeveConsultarRepositorio_QuandoIdForInvalido()
        {
            Record.Exception(() => this.validator.ValidarCancelamento(0));

            this.repositoryMock.Verify(r => r.Get(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ValidarPagamento_DeveLancarExcecao_QuandoIdForZero()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarPagamento(0));

            Assert.Equal("O identificador do lançamento é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void ValidarPagamento_DeveLancarExcecao_QuandoIdForNegativo()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarPagamento(-1));

            Assert.Equal("O identificador do lançamento é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void ValidarPagamento_DeveLancarExcecao_QuandoLancamentoNaoForEncontrado()
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns((LancamentoFinanceiro?)null);

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarPagamento(1));

            Assert.Equal("Lançamento com identificador 1 não encontrado.", excecao.Message);
        }

        [Theory]
        [InlineData(StatusLancamento.Pago)]
        [InlineData(StatusLancamento.Cancelado)]
        public void ValidarPagamento_DeveLancarExcecao_QuandoStatusNaoForAberto(StatusLancamento status)
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(CriarEntidade(status));

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.ValidarPagamento(1));

            Assert.Equal("Apenas lançamentos com status Aberto podem ser pagos.", excecao.Message);
        }

        [Fact]
        public void ValidarPagamento_NaoDeveLancarExcecao_QuandoLancamentoExisteEStatusForAberto()
        {
            Exception excecao = Record.Exception(() => this.validator.ValidarPagamento(1));
            Assert.Null(excecao);
        }

        [Fact]
        public void ValidarPagamento_NaoDeveConsultarRepositorio_QuandoIdForInvalido()
        {
            Record.Exception(() => this.validator.ValidarPagamento(0));

            this.repositoryMock.Verify(r => r.Get(It.IsAny<int>()), Times.Never);
        }
    }
}
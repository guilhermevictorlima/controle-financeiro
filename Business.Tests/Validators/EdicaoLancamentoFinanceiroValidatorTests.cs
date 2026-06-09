namespace Business.Tests.Validators
{
    using Business.Exceptions;
    using Business.Validators;
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;
    using Data.Models.Entities;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;
    using Moq;
    using Xunit;

    public class EdicaoLancamentoFinanceiroValidatorTests
    {
        private readonly Mock<ILancamentoFinanceiroRepository> repositoryMock;
        private readonly EdicaoLancamentoFinanceiroValidator validator;

        public EdicaoLancamentoFinanceiroValidatorTests()
        {
            this.repositoryMock = new Mock<ILancamentoFinanceiroRepository>();
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(CriarEntidade());

            this.validator = new EdicaoLancamentoFinanceiroValidator(this.repositoryMock.Object);
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

        private static EditarLancamentoFinanceiroDTO CriarDTO(
            int id = 1,
            string descricao = "Pagamento de fornecedor",
            TipoLancamento tipo = TipoLancamento.Debito,
            decimal valorOriginal = 100.00m,
            decimal percentualTaxa = 0,
            decimal percentualDesconto = 0,
            decimal valorCalculado = 100.00m,
            DateTime? dataLancamento = null)
        {
            return new EditarLancamentoFinanceiroDTO(
                id,
                descricao,
                tipo,
                valorOriginal,
                percentualTaxa,
                percentualDesconto,
                valorCalculado,
                dataLancamento ?? new DateTime(2024, 6, 15),
                new Competencia("2024-06"));
        }

        private static EditarLancamentoFinanceiroDTO CriarDTO(
            Competencia competencia,
            int id = 1,
            string descricao = "Pagamento de fornecedor",
            TipoLancamento tipo = TipoLancamento.Debito,
            decimal valorOriginal = 100.00m,
            decimal percentualTaxa = 0,
            decimal percentualDesconto = 0,
            decimal valorCalculado = 100.00m,
            DateTime? dataLancamento = null)
        {
            return new EditarLancamentoFinanceiroDTO(
                id,
                descricao,
                tipo,
                valorOriginal,
                percentualTaxa,
                percentualDesconto,
                valorCalculado,
                dataLancamento ?? new DateTime(2024, 6, 15),
                competencia);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoIdForZero()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(id: 0)));

            Assert.Equal("O identificador do lançamento é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoIdForNegativo()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(id: -1)));

            Assert.Equal("O identificador do lançamento é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoDescricaoForNula()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(descricao: null)));

            Assert.Equal("A descrição é obrigatória.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoDescricaoUltrapassar250Caracteres()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(descricao: new string('a', 251))));

            Assert.Equal("A descrição não pode ultrapassar 250 caracteres.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoValorOriginalForZero()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(valorOriginal: 0)));

            Assert.Equal("O valor original é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoValorCalculadoForZero()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(valorCalculado: 0)));

            Assert.Equal("O valor calculado é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoCompetenciaForDefault()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(competencia: default)));

            Assert.Equal("A competência é obrigatória.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoDataLancamentoForDefault()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(dataLancamento: default(DateTime))));

            Assert.Equal("A data de lançamento é obrigatória.", excecao.Message);
        }


        [Fact]
        public void Validar_DeveLancarExcecao_QuandoLancamentoNaoForEncontrado()
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns((LancamentoFinanceiro?)null);

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO()));

            Assert.Contains("não encontrado", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoStatusNaoForAberto()
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(CriarEntidade(StatusLancamento.Pago));

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO()));

            Assert.Equal("Apenas lançamentos com status Aberto podem ser editados.", excecao.Message);
        }

        [Theory]
        [InlineData(StatusLancamento.Pago)]
        [InlineData(StatusLancamento.Cancelado)]
        public void Validar_DeveLancarExcecao_ParaTodosStatusDiferentesDeAberto(StatusLancamento status)
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(CriarEntidade(status));

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO()));

            Assert.Equal("Apenas lançamentos com status Aberto podem ser editados.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecaoDeExistencia_QuandoLancamentoNaoForEncontrado()
        {
            this.repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns((LancamentoFinanceiro?)null);

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO()));

            Assert.Equal("Lançamento com identificador 1 não encontrado.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveConsultarRepositorioExatamenteDuasVezes_AoValidarEdicao()
        {
            this.validator.Validar(CriarDTO());

            this.repositoryMock.Verify(
                r => r.Get(It.IsAny<int>()),
                Times.Once);
        }


        // -------------------------------------------------------------------------
        // Validações de negócio — data e competência
        // -------------------------------------------------------------------------

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoDataLancamentoForIncompativelComCompetencia()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(dataLancamento: new DateTime(2024, 7, 1))));

            Assert.Contains("não é compatível com a competência", excecao.Message);
        }

        // -------------------------------------------------------------------------
        // Validações de negócio — valor calculado
        // -------------------------------------------------------------------------

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoTaxaEDescontoForemInformadosSimultaneamente()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(percentualTaxa: 10, percentualDesconto: 5)));

            Assert.Equal("Taxa e desconto são mutuamente exclusivos. Informe apenas um dos dois.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoTaxaForInformadaParaLancamentoCredito()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(tipo: TipoLancamento.Credito, percentualTaxa: 10, valorCalculado: 110.00m)));

            Assert.Equal("Percentual de taxa só é permitido para lançamentos do tipo Débito.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoDescontoForInformadoParaLancamentoDebito()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(percentualDesconto: 10, valorCalculado: 90.00m)));

            Assert.Equal("Percentual de desconto só é permitido para lançamentos do tipo Crédito.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoValorCalculadoComTaxaForIncorreto()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(percentualTaxa: 10, valorCalculado: 99.00m)));

            Assert.Contains("Valor calculado inválido", excecao.Message);
            Assert.Contains("110,00", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoValorCalculadoComDescontoForIncorreto()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(tipo: TipoLancamento.Credito, percentualDesconto: 10, valorCalculado: 99.00m)));

            Assert.Contains("Valor calculado inválido", excecao.Message);
            Assert.Contains("90,00", excecao.Message);
        }

        [Fact]
        public void Validar_NaoDeveLancarExcecao_QuandoDTOForValido()
        {
            Exception excecao = Record.Exception(
                () => this.validator.Validar(CriarDTO()));

            Assert.Null(excecao);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoLancamentoDuplicadoForDetectado()
        {
            this.repositoryMock
                .Setup(r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()))
                .Returns(true);

            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO()));

            Assert.Equal("Já existe um lançamento com a mesma descrição, tipo e competência.", excecao.Message);
        }

        [Fact]
        public void Validar_NaoDeveLancarExcecao_QuandoNaoHouverLancamentoDuplicado()
        {
            this.repositoryMock
                .Setup(r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()))
                .Returns(false);

            Exception excecao = Record.Exception(
                () => this.validator.Validar(CriarDTO()));

            Assert.Null(excecao);
        }

        [Fact]
        public void Validar_DeveConsultarRepositorio_ComDadosCorretosDoDTO()
        {
            this.repositoryMock
                .Setup(r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()))
                .Returns(false);

            this.validator.Validar(CriarDTO());

            this.repositoryMock.Verify(
                r => r.IsLancamentoDuplicado(It.Is<VerificarLancamentoDuplicadoDTO>(d =>
                    d.Descricao == "Pagamento de fornecedor" &&
                    d.Tipo == TipoLancamento.Debito &&
                    d.Competencia == new Competencia("2024-06"))),
                Times.Once);
        }

        [Fact]
        public void Validar_DeveConsultarRepositorioExatamenteUmaVez_AoValidarLancamento()
        {
            this.validator.Validar(CriarDTO());

            this.repositoryMock.Verify(
                r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()),
                Times.Once);
        }

        [Fact]
        public void Validar_NaoDeveConsultarRepositorio_QuandoValidacaoDeIntegridadeFalhar()
        {
            Record.Exception(
                () => this.validator.Validar(CriarDTO(descricao: null)));

            this.repositoryMock.Verify(
                r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()),
                Times.Never);
        }

        [Fact]
        public void Validar_NaoDeveConsultarRepositorio_QuandoValidacaoDeNegocioFalharAntesDeDuplicidade()
        {
            Record.Exception(
                () => this.validator.Validar(CriarDTO(dataLancamento: new DateTime(2024, 7, 1))));

            this.repositoryMock.Verify(
                r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()),
                Times.Never);
        }
    }
}
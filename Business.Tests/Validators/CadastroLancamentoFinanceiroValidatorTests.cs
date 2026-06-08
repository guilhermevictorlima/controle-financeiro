namespace Business.Tests.Validators
{
    using Business.Exceptions;
    using Business.Validators;
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;
    using Data.Models.DTOs;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;
    using Moq;
    using Xunit;

    public class CadastroLancamentoFinanceiroValidatorTests
    {

        private readonly Mock<ILancamentoFinanceiroRepository> repositoryMock;

        private readonly CadastroLancamentoFinanceiroValidator validator;

        public CadastroLancamentoFinanceiroValidatorTests()
        {
            this.repositoryMock = new Mock<ILancamentoFinanceiroRepository>();
            this.repositoryMock
                .Setup(r => r.IsLancamentoDuplicado(It.IsAny<VerificarLancamentoDuplicadoDTO>()))
                .Returns(false);

            this.validator = new CadastroLancamentoFinanceiroValidator(this.repositoryMock.Object);
        }

        private static CriarLancamentoFinanceiroDTO CriarDTO(
            string descricao = "Pagamento de fornecedor",
            TipoLancamento tipo = TipoLancamento.Debito,
            decimal valorOriginal = 100.00m,
            decimal percentualTaxa = 0,
            decimal percentualDesconto = 0,
            decimal valorCalculado = 100.00m,
            DateTime? dataLancamento = null)
        {
            return new CriarLancamentoFinanceiroDTO(
                descricao,
                tipo,
                valorOriginal,
                percentualTaxa,
                percentualDesconto,
                valorCalculado,
                dataLancamento ?? new DateTime(2024, 6, 15),
                new Competencia("2024-06")
            );
        }

        private static CriarLancamentoFinanceiroDTO CriarDTO(
            Competencia competencia,
            string descricao = "Pagamento de fornecedor",
            TipoLancamento tipo = TipoLancamento.Debito,
            decimal valorOriginal = 100.00m,
            decimal percentualTaxa = 0,
            decimal percentualDesconto = 0,
            decimal valorCalculado = 100.00m,
            DateTime? dataLancamento = null)
        {
            return new CriarLancamentoFinanceiroDTO(
                descricao,
                tipo,
                valorOriginal,
                percentualTaxa,
                percentualDesconto,
                valorCalculado,
                dataLancamento ?? new DateTime(2024, 6, 15),
                competencia
            );
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
        public void Validar_NaoDeveLancarExcecao_QuandoDescricaoPossuirExatamente250Caracteres()
        {
            Exception excecao = Record.Exception(
                () => this.validator.Validar(CriarDTO(descricao: new string('a', 250))));

            Assert.Null(excecao);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoValorOriginalForZero()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(valorOriginal: 0)));

            Assert.Equal("O valor original é obrigatório e deve ser maior que zero.", excecao.Message);
        }

        [Fact]
        public void Validar_DeveLancarExcecao_QuandoValorOriginalForNegativo()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(valorOriginal: -1)));

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
        public void Validar_DeveLancarExcecao_QuandoDataLancamentoForIncompativelComCompetencia()
        {
            LancamentoFinanceiroException excecao = Assert.Throws<LancamentoFinanceiroException>(
                () => this.validator.Validar(CriarDTO(dataLancamento: new DateTime(2024, 7, 1))));

            Assert.Contains("não é compatível com a competência", excecao.Message);
        }

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
        public void Validar_NaoDeveLancarExcecao_QuandoValorCalculadoComTaxaForCorreto()
        {
            Exception excecao = Record.Exception(
                () => this.validator.Validar(CriarDTO(percentualTaxa: 10, valorCalculado: 110.00m)));

            Assert.Null(excecao);
        }

        [Fact]
        public void Validar_NaoDeveLancarExcecao_QuandoValorCalculadoComDescontoForCorreto()
        {
            Exception excecao = Record.Exception(
                () => this.validator.Validar(CriarDTO(tipo: TipoLancamento.Credito, percentualDesconto: 10, valorCalculado: 90.00m)));

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
        public void Validar_DeveConsultarRepositorio_ComDadosCorretosDODTO()
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
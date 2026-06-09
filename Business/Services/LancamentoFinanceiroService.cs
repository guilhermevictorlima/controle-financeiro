namespace Business.Services
{
    using Business.Exporters;
    using Business.Validators;
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;
    using Data.Repositories;

    public class LancamentoFinanceiroService
    {
        private static readonly LancamentoFinanceiroRepository Repository = new();
        private static readonly CadastroLancamentoFinanceiroValidator CadastroValidator = new(Repository);
        private static readonly EdicaoLancamentoFinanceiroValidator EdicaoValidator = new(Repository);
        private static readonly AlteracaoStatusLancamentoValidator AlteracaoStatusValidator = new(Repository);

        public void CadastrarLancamento(CriarLancamentoFinanceiroDTO dto)
        {
            CadastroValidator.Validar(dto);
            Repository.Save(dto);
        }

        public void EditarLancamento(EditarLancamentoFinanceiroDTO dto)
        {
            EdicaoValidator.Validar(dto);
            Repository.Update(dto);
        }

        public void CancelarLancamento(int id)
        {
            AlteracaoStatusValidator.ValidarCancelamento(id);
            Repository.Cancelar(id);
        }

        public void PagarLancamento(int id)
        {
            AlteracaoStatusValidator.ValidarPagamento(id);
            Repository.Pagar(id);
        }

        public List<LancamentoFinanceiroResponseDTO> Listar()
        {
            return Repository.Listar().ToList();
        }

        public ArquivoExportadoDTO ExportarLancamentos(ExportarLancamentosDTO dto)
        {
            var competencia = new Competencia(dto.Competencia);
            IReadOnlyList<LancamentoFinanceiroResponseDTO> registros = Repository.ListarPorCompetencia(competencia);
            
            return LancamentoFinanceiroExporterFactory.Criar(dto.TipoExportacao)
                    .Exportar(registros, competencia);
        }

    }
}
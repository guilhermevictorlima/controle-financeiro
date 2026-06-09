namespace Business.Services
{
    using Business.Exporters;
    using Business.Validators;
    using Data.Core.DTOs;
    using Data.Repositories;

    public class LancamentoFinanceiroService
    {
        private static readonly LancamentoFinanceiroRepository Repository = new();
        private static readonly CadastroLancamentoFinanceiroValidator CadastroValidator = new(Repository);
        private static readonly EdicaoLancamentoFinanceiroValidator EdicaoValidator = new(Repository);
        private static readonly AlteracaoStatusLancamentoValidator AlteracaoStatusValidator = new(Repository);

        public LancamentoFinanceiroResponseDTO CadastrarLancamento(CriarLancamentoFinanceiroDTO dto)
        {
            CadastroValidator.Validar(dto);
            return LancamentoFinanceiroResponseDTO.FromEntity(Repository.Save(dto));
        }

        public LancamentoFinanceiroResponseDTO EditarLancamento(EditarLancamentoFinanceiroDTO dto)
        {
            EdicaoValidator.Validar(dto);
            return LancamentoFinanceiroResponseDTO.FromEntity(Repository.Update(dto));
        }

        public LancamentoFinanceiroResponseDTO CancelarLancamento(int id)
        {
            AlteracaoStatusValidator.ValidarCancelamento(id);
            Repository.Cancelar(id);
            return LancamentoFinanceiroResponseDTO.FromEntity(Repository.Get(id)!);
        }

        public LancamentoFinanceiroResponseDTO PagarLancamento(int id)
        {
            AlteracaoStatusValidator.ValidarPagamento(id);
            Repository.Pagar(id);
            return LancamentoFinanceiroResponseDTO.FromEntity(Repository.Get(id)!);
        }

        public ArquivoExportadoDTO ExportarLancamentos(ExportarLancamentosDTO dto)
        {
            IReadOnlyList<LancamentoFinanceiroResponseDTO> registros = Repository.ListarPorCompetencia(dto.Competencia);
            
            return LancamentoFinanceiroExporterFactory.Criar(dto.TipoExportacao)
                    .Exportar(registros, dto.Competencia);
        }

    }
}
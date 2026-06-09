namespace Business.Exporters
{
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;

    internal interface ILancamentoFinanceiroExporter
    {
        ArquivoExportadoDTO Exportar(IReadOnlyList<LancamentoFinanceiroResponseDTO> registros, Competencia competencia);
    }
}
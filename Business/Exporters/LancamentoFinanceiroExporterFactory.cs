namespace Business.Exporters
{
    using Data.Core.Enums;

    internal static class LancamentoFinanceiroExporterFactory
    {
        internal static ILancamentoFinanceiroExporter Criar(TipoExportacao tipo)
        {
            return tipo switch
            {
                TipoExportacao.Csv => new CsvLancamentoFinanceiroExporter(),
                TipoExportacao.Excel => new ExcelLancamentoFinanceiroExporter(),
                _ => throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo de exportação não suportado."),
            };
        }
    }
}
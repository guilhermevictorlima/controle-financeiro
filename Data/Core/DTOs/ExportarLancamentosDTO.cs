namespace Data.Core.DTOs
{
    using Data.Core.Enums;

    public record ExportarLancamentosDTO(
        string Competencia,
        TipoExportacao TipoExportacao);
}

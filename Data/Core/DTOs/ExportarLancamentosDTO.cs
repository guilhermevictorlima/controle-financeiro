namespace Data.Core.DTOs
{
    using Data.Core.Enums;
    using Data.Core.ValueObjects;

    public record ExportarLancamentosDTO(
        Competencia Competencia,
        TipoExportacao TipoExportacao);
}

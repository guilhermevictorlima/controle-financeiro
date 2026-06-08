namespace Data.Models.DTOs
{
    using Data.Core.ValueObjects;
    using Data.Models.Enums;

    public record CriarLancamentoFinanceiroDTO(
        string Descricao,
        TipoLancamento Tipo,
        decimal ValorOriginal,
        decimal PercentualTaxa,
        decimal PercentualDesconto,
        decimal ValorCalculado,
        DateTime DataLancamento,
        Competencia Competencia);
}

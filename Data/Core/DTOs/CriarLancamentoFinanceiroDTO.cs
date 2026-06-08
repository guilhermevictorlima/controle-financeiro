namespace Data.Models.DTOs
{
    using Data.Models.Enums;

    public record CriarLancamentoFinanceiroDTO(
        string Descricao,
        TipoLancamento Tipo,
        decimal ValorOriginal,
        decimal PercentualTaxa,
        decimal PercentualDesconto,
        decimal ValorCalculado,
        DateTime DataLancamento,
        string Competencia);
}

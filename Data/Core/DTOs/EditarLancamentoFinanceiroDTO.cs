namespace Data.Core.DTOs
{
    using Data.Core.ValueObjects;
    using Data.Models.Enums;

    public record EditarLancamentoFinanceiroDTO(
        int Id,
        string Descricao,
        TipoLancamento Tipo,
        decimal ValorOriginal,
        decimal PercentualTaxa,
        decimal PercentualDesconto,
        decimal ValorCalculado,
        DateTime DataLancamento) : IDadosLancamentoFinanceiro
    {
        public Competencia Competencia => new ($"{DataLancamento:yyyy-MM}");
    }
}
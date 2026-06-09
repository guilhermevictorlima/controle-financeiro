namespace Data.Core.DTOs
{
    using Data.Core.ValueObjects;
    using Data.Models.Entities;
    using Data.Models.Enums;

    public record LancamentoFinanceiroResponseDTO(
        int Id,
        string Descricao,
        TipoLancamento Tipo,
        decimal ValorOriginal,
        decimal PercentualTaxa,
        decimal PercentualDesconto,
        decimal ValorCalculado,
        DateTime DataLancamento,
        DateTime DataCriacao,
        DateTime? DataPagamento,
        DateTime? DataCancelamento,
        Competencia Competencia,
        StatusLancamento Status)
    {
        public static LancamentoFinanceiroResponseDTO FromEntity(LancamentoFinanceiro entity)
        {
            return new LancamentoFinanceiroResponseDTO(
                Id: entity.Id,
                Descricao: entity.Descricao,
                Tipo: entity.Tipo,
                ValorOriginal: entity.ValorOriginal,
                PercentualTaxa: entity.PercentualTaxa,
                PercentualDesconto: entity.PercentualDesconto,
                ValorCalculado: entity.ValorCalculado,
                DataLancamento: entity.DataLancamento,
                DataCriacao: entity.DataCriacao,
                DataPagamento: entity.DataPagamento,
                DataCancelamento: entity.DataCancelamento,
                Competencia: entity.Competencia,
                Status: entity.Status);
        }
    }
}
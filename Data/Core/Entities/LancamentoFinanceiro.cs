namespace Data.Models.Entities
{
    using Data.Core.ValueObjects;
    using Data.Models.Enums;

    public class LancamentoFinanceiro
    {
        required public int Id { get; init; }

        required public string Descricao { get; set; }

        required public TipoLancamento Tipo { get; set; }

        required public decimal ValorOriginal { get; set; }

        required public decimal PercentualTaxa { get; set; }

        required public decimal PercentualDesconto { get; set; }

        required public decimal ValorCalculado { get; set; }

        required public DateTime DataLancamento { get; set; }

        required public DateTime DataCriacao { get; set; }

        public DateTime? DataPagamento { get; set; }

        public DateTime? DataCancelamento { get; set; }

        required public Competencia Competencia { get; set; }

        required public StatusLancamento Status { get; set; }
    }
}
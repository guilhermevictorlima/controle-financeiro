namespace Data.Core.DTOs
{
    using Data.Core.ValueObjects;
    using Data.Models.Enums;

    public interface IDadosLancamentoFinanceiro
    {
        string Descricao { get; }

        TipoLancamento Tipo { get; }

        decimal ValorOriginal { get; }

        decimal PercentualTaxa { get; }

        decimal PercentualDesconto { get; }

        decimal ValorCalculado { get; }

        DateTime DataLancamento { get; }

        Competencia Competencia { get; }
    }
}

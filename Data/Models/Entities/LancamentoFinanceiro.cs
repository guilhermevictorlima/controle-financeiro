namespace Data.Models.Entities
{
    using System.Text.RegularExpressions;
    using Data.Models.Enums;
    using Data.Models.Validators;

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

        required public string Competencia
        {
            get;

            set
            {
                CompetenciaValidator.Validate(value);
                field = value;
            }
        }

        required public StatusLancamento Status { get; set; }
    }
}
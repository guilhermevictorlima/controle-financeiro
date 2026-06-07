namespace Data.Models.Entities
{
    using Data.Models.Enums;
    using System.Text.RegularExpressions;

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
            get
            {
                return this.Competencia;
            }

            set
            {
                string regexPadraoCompetencia = @"^\d{4}-(0[1-9]|1[0-2])$";
                if (!Regex.IsMatch(value, regexPadraoCompetencia))
                {
                    throw new ArgumentException("A competência deve estar no formato YYYY-MM.", nameof(value));
                }
            }
        }

        required public StatusLancamento Status { get; set; }
    }
}
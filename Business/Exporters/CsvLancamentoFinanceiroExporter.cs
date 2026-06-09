namespace Business.Exporters
{
    using System.Text;
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;

    internal class CsvLancamentoFinanceiroExporter : ILancamentoFinanceiroExporter
    {
        private const string Separador = ";";
        private const string ContentType = "text/csv";

        public ArquivoExportadoDTO Exportar(IReadOnlyList<LancamentoFinanceiroResponseDTO> registros, Competencia competencia)
        {
            StringBuilder csv = new ();

            csv.AppendLine(this.MontarCabecalho());

            foreach (LancamentoFinanceiroResponseDTO registro in registros)
            {
                csv.AppendLine(this.MontarLinha(registro));
            }

            byte[] conteudo = Encoding.UTF8.GetBytes(csv.ToString());
            string nomeArquivo = $"lancamentos_{competencia}.csv";

            return new ArquivoExportadoDTO(conteudo, nomeArquivo, ContentType);
        }

        private string MontarCabecalho()
        {
            return string.Join(Separador,
                "ID",
                "Descrição",
                "Tipo",
                "Valor Original",
                "% Taxa",
                "% Desconto",
                "Valor Calculado",
                "Data Lançamento",
                "Data Criação",
                "Data Pagamento",
                "Data Cancelamento",
                "Competência",
                "Status");
        }

        private string MontarLinha(LancamentoFinanceiroResponseDTO registro)
        {
            return string.Join(Separador,
                registro.Id,
                registro.Descricao,
                registro.Tipo,
                registro.ValorOriginal.ToString("F2"),
                registro.PercentualTaxa.ToString("F2"),
                registro.PercentualDesconto.ToString("F2"),
                registro.ValorCalculado.ToString("F2"),
                registro.DataLancamento.ToString("dd/MM/yyyy"),
                registro.DataCriacao.ToString("dd/MM/yyyy"),
                registro.DataPagamento?.ToString("dd/MM/yyyy"),
                registro.DataCancelamento?.ToString("dd/MM/yyyy"),
                registro.Competencia,
                registro.Status);
        }
    }
}
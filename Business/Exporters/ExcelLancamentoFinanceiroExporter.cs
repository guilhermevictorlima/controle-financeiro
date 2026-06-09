namespace Business.Exporters
{
    using ClosedXML.Excel;
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;

    internal class ExcelLancamentoFinanceiroExporter : ILancamentoFinanceiroExporter
    {
        private const string NomeAba = "Lançamentos";
        private const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ArquivoExportadoDTO Exportar(IReadOnlyList<LancamentoFinanceiroResponseDTO> registros, Competencia competencia)
        {
            using XLWorkbook workbook = new();
            IXLWorksheet worksheet = workbook.Worksheets.Add(NomeAba);

            this.MontarCabecalho(worksheet);
            this.MontarLinhas(worksheet, registros);

            worksheet.Columns().AdjustToContents();

            using MemoryStream stream = new();
            workbook.SaveAs(stream);

            string nomeArquivo = $"lancamentos_{competencia}.xlsx";

            return new ArquivoExportadoDTO(stream.ToArray(), nomeArquivo, ContentType);
        }

        private void MontarCabecalho(IXLWorksheet worksheet)
        {
            string[] colunas =
            [
                "ID", "Descrição", "Tipo", "Valor Original", "% Taxa", "% Desconto",
                "Valor Calculado", "Data Lançamento", "Data Criação", "Data Pagamento",
                "Data Cancelamento", "Competência", "Status",
            ];

            for (int i = 0; i < colunas.Length; i++)
            {
                IXLCell celula = worksheet.Cell(1, i + 1);
                celula.Value = colunas[i];
                celula.Style.Font.Bold = true;
            }
        }

        private void MontarLinhas(IXLWorksheet worksheet, IReadOnlyList<LancamentoFinanceiroResponseDTO> registros)
        {
            for (int i = 0; i < registros.Count; i++)
            {
                LancamentoFinanceiroResponseDTO registro = registros[i];
                int linha = i + 2;

                worksheet.Cell(linha, 1).Value = registro.Id;
                worksheet.Cell(linha, 2).Value = registro.Descricao;
                worksheet.Cell(linha, 3).Value = registro.Tipo.ToString();
                worksheet.Cell(linha, 4).Value = registro.ValorOriginal;
                worksheet.Cell(linha, 5).Value = registro.PercentualTaxa;
                worksheet.Cell(linha, 6).Value = registro.PercentualDesconto;
                worksheet.Cell(linha, 7).Value = registro.ValorCalculado;
                worksheet.Cell(linha, 8).Value = registro.DataLancamento.ToString("dd/MM/yyyy");
                worksheet.Cell(linha, 9).Value = registro.DataCriacao.ToString("dd/MM/yyyy");
                worksheet.Cell(linha, 10).Value = registro.DataPagamento?.ToString("dd/MM/yyyy") ?? string.Empty;
                worksheet.Cell(linha, 11).Value = registro.DataCancelamento?.ToString("dd/MM/yyyy") ?? string.Empty;
                worksheet.Cell(linha, 12).Value = registro.Competencia.ToString();
                worksheet.Cell(linha, 13).Value = registro.Status.ToString();
            }
        }
    }
}
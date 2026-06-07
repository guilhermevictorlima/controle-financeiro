namespace Data.Repositories
{
    using Data.Models.Entities;
    using Data.Models.Enums;
    using Microsoft.Data.SqlClient;

    public class LancamentoFinanceiroRepository : RepositoryBase<LancamentoFinanceiro>
    {
        public string Get()
        {
            throw new NotImplementedException();
        }

        internal override LancamentoFinanceiro EntityResultMapper(SqlDataReader reader)
        {
            return new LancamentoFinanceiro
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Descricao = reader.GetString(reader.GetOrdinal("descricao")),
                Tipo = Enum.Parse<TipoLancamento>(reader.GetString(reader.GetOrdinal("tipo"))),
                ValorOriginal = reader.GetDecimal(reader.GetOrdinal("valor_original")),
                PercentualTaxa = reader.GetDecimal(reader.GetOrdinal("percentual_taxa")),
                PercentualDesconto = reader.GetDecimal(reader.GetOrdinal("percentual_desconto")),
                ValorCalculado = reader.GetDecimal(reader.GetOrdinal("valor_calculado")),
                DataLancamento = reader.GetDateTime(reader.GetOrdinal("data_lancamento")),
                DataCriacao = reader.GetDateTime(reader.GetOrdinal("data_criacao")),
                DataPagamento = reader.IsDBNull(reader.GetOrdinal("data_pagamento")) ? null : reader.GetDateTime(reader.GetOrdinal("data_pagamento")),
                DataCancelamento = reader.IsDBNull(reader.GetOrdinal("data_cancelamento")) ? null : reader.GetDateTime(reader.GetOrdinal("data_cancelamento")),
                Competencia = reader.GetString(reader.GetOrdinal("competencia")),
                Status = Enum.Parse<StatusLancamento>(reader.GetString(reader.GetOrdinal("status"))),
            };
        }
    }
}

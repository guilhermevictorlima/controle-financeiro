namespace Data.Repositories
{
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;
    using Data.Models.Entities;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;
    using Microsoft.Data.SqlClient;

    public class LancamentoFinanceiroRepository : RepositoryBase<LancamentoFinanceiro>, ILancamentoFinanceiroRepository
    {

        public IReadOnlyList<LancamentoFinanceiroResponseDTO> ListarPorCompetencia(Competencia competencia)
        {
            return this.List($"""
                select *
                from lancamento_financeiro
                where competencia = '{competencia}'
                order by data_lancamento
            """).Select(LancamentoFinanceiroResponseDTO.FromEntity)
                .ToList()
                .AsReadOnly();
        }

        public LancamentoFinanceiro? Get(int id)
        {
            return this.Get($"""
                select *
                from lancamento_financeiro
                where id = {id}
                """);
        }

        public LancamentoFinanceiro Save(CriarLancamentoFinanceiroDTO dto)
        {
            string sql = """
                insert into lancamento_financeiro (
                    descricao,
                    tipo,
                    valor_original,
                    percentual_taxa,
                    percentual_desconto,
                    valor_calculado,
                    data_lancamento,
                    data_criacao,
                    competencia
                )
                values (
                    @descricao,
                    @tipo,
                    @valor_original,
                    @percentual_taxa,
                    @percentual_desconto,
                    @valor_calculado,
                    @data_lancamento,
                    @data_criacao,
                    @competencia
                )
                """;

            Dictionary<string, object> parameters = new ()
            {
                { "@descricao",           dto.Descricao },
                { "@tipo",                dto.Tipo.ToString() },
                { "@valor_original",      dto.ValorOriginal },
                { "@percentual_taxa",     dto.PercentualTaxa },
                { "@percentual_desconto", dto.PercentualDesconto },
                { "@valor_calculado",     dto.ValorCalculado },
                { "@data_lancamento",     dto.DataLancamento },
                { "@data_criacao",        DateTime.Now },
                { "@competencia",         dto.Competencia.ToString() },
            };

            this.Persist(sql, parameters);

            // return this.GetLastInserted();
            return default;
        }

        public LancamentoFinanceiro Update(EditarLancamentoFinanceiroDTO dto)
        {
            string sql = """
                update lancamento_financeiro set
                    descricao           = @descricao,
                    tipo                = @tipo,
                    valor_original      = @valor_original,
                    percentual_taxa     = @percentual_taxa,
                    percentual_desconto = @percentual_desconto,
                    valor_calculado     = @valor_calculado,
                    data_lancamento     = @data_lancamento,
                    competencia         = @competencia
                where id = @id
            """;

            Dictionary<string, object> parameters = new ()
            {
                { "@id",                  dto.Id                        },
                { "@descricao",           dto.Descricao                 },
                { "@tipo",                dto.Tipo.ToString()           },
                { "@valor_original",      dto.ValorOriginal             },
                { "@percentual_taxa",     dto.PercentualTaxa            },
                { "@percentual_desconto", dto.PercentualDesconto        },
                { "@valor_calculado",     dto.ValorCalculado            },
                { "@data_lancamento",     dto.DataLancamento            },
                { "@competencia",         dto.Competencia.ToString()    },
            };

            this.Persist(sql, parameters);

            // return this.GetLastInserted();
            return default;
        }

        public void Cancelar(int id)
        {
            string sql = """
                update lancamento_financeiro set
                    status            = @status,
                    data_cancelamento = @data_cancelamento
                where id = @id
            """;

            Dictionary<string, object> parameters = new()
            {
                { "@id",                id                                      },
                { "@status",            StatusLancamento.Cancelado.ToString()   },
                { "@data_cancelamento", DateTime.Now                            },
            };

            this.Persist(sql, parameters);
        }

        public void Pagar(int id)
        {
            string sql = """
                update lancamento_financeiro set
                    status         = @status,
                    data_pagamento = @data_pagamento
                where id = @id
            """;

            Dictionary<string, object> parameters = new()
            {
                { "@id",             id                                 },
                { "@status",         StatusLancamento.Pago.ToString()   },
                { "@data_pagamento", DateTime.Now                       },
            };

            this.Persist(sql, parameters);
        }

        public bool IsLancamentoDuplicado(VerificarLancamentoDuplicadoDTO dto)
        {
            return this.List($"""
                select count(1)
                from lancamento_financeiro
                where descricao = {dto.Descricao}
                  and tipo = {dto.Tipo.ToString()}
                  and competencia = {dto.Competencia.ToString()}
                """).Count != 0;
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
                Competencia = new Competencia(reader.GetString(reader.GetOrdinal("competencia"))),
                Status = Enum.Parse<StatusLancamento>(reader.GetString(reader.GetOrdinal("status"))),
            };
        }
    }
}

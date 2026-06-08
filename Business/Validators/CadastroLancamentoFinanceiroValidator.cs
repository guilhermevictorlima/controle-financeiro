namespace Business.Validators
{
    using Business.Exceptions;
    using Data.Core.DTOs;
    using Data.Models.DTOs;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;

    internal class CadastroLancamentoFinanceiroValidator(ILancamentoFinanceiroRepository Repository)
    {
        internal void Validar(CriarLancamentoFinanceiroDTO dto)
        {
            this.RealizarValidacoesDeIntegridade(dto);
            this.RealizarValidacoesDeNegocio(dto);
        }

        private void RealizarValidacoesDeIntegridade(CriarLancamentoFinanceiroDTO dto)
        {
            Dictionary<string, bool> violacoesDeIntegridade = new()
            {
                { "A descrição é obrigatória.",                                dto.Descricao == null          },
                { "A descrição não pode ultrapassar 250 caracteres.",          dto.Descricao?.Length > 250    },
                { "O valor original é obrigatório e deve ser maior que zero.", dto.ValorOriginal <= 0         },
                { "O valor calculado é obrigatório e deve ser maior que zero.", dto.ValorCalculado <= 0       },
                { "A competência é obrigatória.",                              dto.Competencia == default     },
                { "A data de lançamento é obrigatória.",                       dto.DataLancamento == default  },
            };

            this.ExecutarValidacoes(violacoesDeIntegridade);
        }

        private void RealizarValidacoesDeNegocio(CriarLancamentoFinanceiroDTO dto)
        {
            Dictionary<string, bool> violacoesDeNegocio = new();

            this.AdicionarValidacaoDataLancamentoCompativelComCompetencia(dto, violacoesDeNegocio);
            this.AdicionarValidacaoValorCalculado(dto, violacoesDeNegocio);
            this.ExecutarValidacoes(violacoesDeNegocio);

            this.ValidarDuplicidadeLancamento(dto);
        }

        private void ExecutarValidacoes(Dictionary<string, bool> violacoes)
        {
            foreach (KeyValuePair<string, bool> violacao in violacoes)
            {
                if (violacao.Value)
                    throw new LancamentoFinanceiroException(violacao.Key);
            }
        }

        private void AdicionarValidacaoDataLancamentoCompativelComCompetencia(CriarLancamentoFinanceiroDTO dto, Dictionary<string, bool> violacoes)
        {
            DateTime competenciaData = DateTime.ParseExact(dto.Competencia.ToString(), "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);

            bool incompativel = dto.DataLancamento.Year != competenciaData.Year
                             || dto.DataLancamento.Month != competenciaData.Month;

            violacoes.Add(
                $"A data de lançamento {dto.DataLancamento:dd/MM/yyyy} não é compatível com a competência '{dto.Competencia}'.",
                incompativel
            );
        }

        private void AdicionarValidacaoValorCalculado(CriarLancamentoFinanceiroDTO dto, Dictionary<string, bool> violacoes)
        {
            bool temTaxa = dto.PercentualTaxa > 0;
            bool temDesconto = dto.PercentualDesconto > 0;

            violacoes.Add("Taxa e desconto são mutuamente exclusivos. Informe apenas um dos dois.", temTaxa && temDesconto);
            violacoes.Add("Percentual de taxa só é permitido para lançamentos do tipo Débito.", temTaxa && dto.Tipo != TipoLancamento.Debito);
            violacoes.Add("Percentual de desconto só é permitido para lançamentos do tipo Crédito.", temDesconto && dto.Tipo != TipoLancamento.Credito);

            decimal valorEsperado = dto.ValorOriginal;

            if (temTaxa)
            {
                valorEsperado += dto.ValorOriginal * (dto.PercentualTaxa / 100m);
            }
            else if (temDesconto)
            {
                valorEsperado -= dto.ValorOriginal * (dto.PercentualDesconto / 100m);
            }

            violacoes.Add(
                $"Valor calculado inválido. Esperado: {valorEsperado:F2}, informado: {dto.ValorCalculado:F2}.",
                dto.ValorCalculado != valorEsperado
            );
        }

        private void ValidarDuplicidadeLancamento(CriarLancamentoFinanceiroDTO dto)
        {
            VerificarLancamentoDuplicadoDTO verificarDuplicadoDTO = new(dto.Descricao, dto.Tipo, dto.Competencia);

            if (Repository.IsLancamentoDuplicado(verificarDuplicadoDTO))
                throw new LancamentoFinanceiroException("Já existe um lançamento com a mesma descrição, tipo e competência.");
        }
    }
}
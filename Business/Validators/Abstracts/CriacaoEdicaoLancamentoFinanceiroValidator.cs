using Business.Exceptions;
using Data.Core.DTOs;
using Data.Models.Enums;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Business.Validators.Abstracts
{
    internal abstract class CriacaoEdicaoLancamentoFinanceiroValidator(ILancamentoFinanceiroRepository Repository) : LancamentoFinanceiroValidator(Repository)
    {
        protected void AdicionarValidacoesDeIntegridadeBase(IDadosLancamentoFinanceiro dto, Dictionary<string, bool> violacoes)
        {
            violacoes.Add("A descrição é obrigatória.", dto.Descricao == null);
            violacoes.Add("A descrição não pode ultrapassar 250 caracteres.", dto.Descricao?.Length > 250);
            violacoes.Add("O valor original é obrigatório e deve ser maior que zero.", dto.ValorOriginal <= 0);
            violacoes.Add("O valor calculado é obrigatório e deve ser maior que zero.", dto.ValorCalculado <= 0);
            violacoes.Add("A competência é obrigatória.", dto.Competencia == default);
            violacoes.Add("A data de lançamento é obrigatória.", dto.DataLancamento == default);
        }

        protected void AdicionarValidacoesDeNegocioBase(IDadosLancamentoFinanceiro dto, Dictionary<string, bool> violacoes)
        {
            this.AdicionarValidacaoDataLancamentoCompativelComCompetencia(dto, violacoes);
            this.AdicionarValidacaoValorCalculado(dto, violacoes);
        }

        protected void AdicionarValidacaoDataLancamentoCompativelComCompetencia(IDadosLancamentoFinanceiro dto, Dictionary<string, bool> violacoes)
        {
            DateTime competenciaData = DateTime.ParseExact(dto.Competencia.ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

            bool incompativel = dto.DataLancamento.Year != competenciaData.Year
                             || dto.DataLancamento.Month != competenciaData.Month;

            violacoes.Add(
                $"A data de lançamento {dto.DataLancamento:dd/MM/yyyy} não é compatível com a competência '{dto.Competencia}'.",
                incompativel);
        }

        protected void AdicionarValidacaoValorCalculado(IDadosLancamentoFinanceiro dto, Dictionary<string, bool> violacoes)
        {
            bool temTaxa = dto.PercentualTaxa > 0;
            bool temDesconto = dto.PercentualDesconto > 0;

            violacoes.Add("Taxa e desconto são mutuamente exclusivos. Informe apenas um dos dois.", temTaxa && temDesconto);
            violacoes.Add("Percentual de taxa só é permitido para lançamentos do tipo Débito.", temTaxa && dto.Tipo != TipoLancamento.Debito);
            violacoes.Add("Percentual de desconto só é permitido para lançamentos do tipo Crédito.", temDesconto && dto.Tipo != TipoLancamento.Credito);

            decimal valorEsperado = dto.ValorOriginal;

            if (temTaxa)
                valorEsperado += dto.ValorOriginal * (dto.PercentualTaxa / 100m);
            else if (temDesconto)
                valorEsperado -= dto.ValorOriginal * (dto.PercentualDesconto / 100m);

            violacoes.Add(
                $"Valor calculado inválido. Esperado: {valorEsperado:F2}, informado: {dto.ValorCalculado:F2}.",
                dto.ValorCalculado != valorEsperado);
        }

        protected void ValidarDuplicidadeLancamento(IDadosLancamentoFinanceiro dto)
        {
            VerificarLancamentoDuplicadoDTO verificarDuplicadoDTO = new(dto.Descricao, dto.Tipo, dto.Competencia);

            if (this.Repository.IsLancamentoDuplicado(verificarDuplicadoDTO))
                throw new LancamentoFinanceiroException("Já existe um lançamento com a mesma descrição, tipo e competência.");
        }
    }
}

namespace Business.Validators
{
    using Business.Validators.Abstracts;
    using Data.Core.DTOs;
    using Data.Models.Entities;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;

    internal class EdicaoLancamentoFinanceiroValidator(ILancamentoFinanceiroRepository repository)
        : LancamentoFinanceiroValidator(repository)
    {
        internal void Validar(EditarLancamentoFinanceiroDTO dto)
        {
            this.RealizarValidacoesDeIntegridade(dto);
            this.RealizarValidacoesDeNegocio(dto);
        }

        private void RealizarValidacoesDeIntegridade(EditarLancamentoFinanceiroDTO dto)
        {
            Dictionary<string, bool> violacoesDeIntegridade = new ()
            {
                { "O identificador do lançamento é obrigatório e deve ser maior que zero.", dto.Id <= 0 },
            };

            this.AdicionarValidacoesDeIntegridadeBase(dto, violacoesDeIntegridade);
            this.ExecutarValidacoes(violacoesDeIntegridade);
        }

        private void RealizarValidacoesDeNegocio(EditarLancamentoFinanceiroDTO dto)
        {
            LancamentoFinanceiro? lancamento = this.Repository.Get(dto.Id);

            Dictionary<string, bool> violacoesDeNegocio = new ()
            {
                { $"Lançamento com identificador {dto.Id} não encontrado.", lancamento is null },
                { "Apenas lançamentos com status Aberto podem ser editados.", lancamento is not null && lancamento.Status != StatusLancamento.Aberto }
            };

            this.AdicionarValidacoesDeNegocioBase(dto, violacoesDeNegocio);
            this.ExecutarValidacoes(violacoesDeNegocio);

            this.ValidarDuplicidadeLancamento(dto);
        }
    }
}
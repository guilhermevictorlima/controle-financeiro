namespace Business.Validators
{
    using Business.Validators.Abstracts;
    using Data.Models.Entities;
    using Data.Models.Enums;
    using Data.Repositories.Interfaces;

    internal class AlteracaoStatusLancamentoValidator(ILancamentoFinanceiroRepository repository)
        : LancamentoFinanceiroValidator(repository)
    {
        internal void ValidarCancelamento(int id)
        {
            this.RealizarValidacoesDeIntegridade(id);
            this.RealizarValidacoesDeNegocio(id, "Apenas lançamentos com status Aberto podem ser cancelados.");
        }

        internal void ValidarPagamento(int id)
        {
            this.RealizarValidacoesDeIntegridade(id);
            this.RealizarValidacoesDeNegocio(id, "Apenas lançamentos com status Aberto podem ser pagos.");
        }

        private void RealizarValidacoesDeIntegridade(int id)
        {
            Dictionary<string, bool> violacoesDeIntegridade = new()
            {
                { "O identificador do lançamento é obrigatório e deve ser maior que zero.", id <= 0 },
            };

            this.ExecutarValidacoes(violacoesDeIntegridade);
        }

        private void RealizarValidacoesDeNegocio(int id, string mensagemStatusInvalido)
        {
            LancamentoFinanceiro? lancamento = this.Repository.Get(id);

            Dictionary<string, bool> violacoesDeNegocio = new()
            {
                { $"Lançamento com identificador {id} não encontrado.", lancamento is null                                                  },
                { mensagemStatusInvalido,                               lancamento is not null && lancamento.Status != StatusLancamento.Aberto },
            };

            this.ExecutarValidacoes(violacoesDeNegocio);
        }
    }
}
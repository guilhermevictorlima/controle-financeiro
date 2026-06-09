namespace Business.Validators
{
    using Business.Validators.Abstracts;
    using Data.Core.DTOs;
    using Data.Repositories.Interfaces;

    internal class CadastroLancamentoFinanceiroValidator(ILancamentoFinanceiroRepository repository)
        : CriacaoEdicaoLancamentoFinanceiroValidator(repository)
    {
        internal void Validar(CriarLancamentoFinanceiroDTO dto)
        {
            this.RealizarValidacoesDeIntegridade(dto);
            this.RealizarValidacoesDeNegocio(dto);
        }

        private void RealizarValidacoesDeIntegridade(CriarLancamentoFinanceiroDTO dto)
        {
            Dictionary<string, bool> violacoesDeIntegridade = new();

            this.AdicionarValidacoesDeIntegridadeBase(dto, violacoesDeIntegridade);
            this.ExecutarValidacoes(violacoesDeIntegridade);
        }

        private void RealizarValidacoesDeNegocio(CriarLancamentoFinanceiroDTO dto)
        {
            Dictionary<string, bool> violacoesDeNegocio = new ();

            this.AdicionarValidacoesDeNegocioBase(dto, violacoesDeNegocio);
            this.ExecutarValidacoes(violacoesDeNegocio);

            this.ValidarDuplicidadeLancamento(dto);
        }
    }
}
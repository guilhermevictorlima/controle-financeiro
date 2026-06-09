namespace Data.Repositories.Interfaces
{
    using Data.Core.DTOs;
    using Data.Models.Entities;

    public interface ILancamentoFinanceiroRepository
    {
        LancamentoFinanceiro? Get(int id); // TODO refactor

        LancamentoFinanceiro? Update(EditarLancamentoFinanceiroDTO dto);

        void Cancelar(int id);

        void Pagar(int id);

        bool IsLancamentoDuplicado(VerificarLancamentoDuplicadoDTO dto);
    }
}
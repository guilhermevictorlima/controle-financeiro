namespace Data.Repositories.Interfaces
{
    using Data.Core.DTOs;

    public interface ILancamentoFinanceiroRepository
    {
        bool IsLancamentoDuplicado(VerificarLancamentoDuplicadoDTO dto);
    }
}
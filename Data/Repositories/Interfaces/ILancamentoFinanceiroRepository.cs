namespace Data.Repositories.Interfaces
{
    using Data.Core.DTOs;
    using Data.Core.ValueObjects;
    using Data.Models.Entities;

    public interface ILancamentoFinanceiroRepository
    {
        IReadOnlyList<LancamentoFinanceiroResponseDTO> Listar();
     
        IReadOnlyList<LancamentoFinanceiroResponseDTO> ListarPorCompetencia(Competencia competencia);

        LancamentoFinanceiro? Get(int id);

        void Update(EditarLancamentoFinanceiroDTO dto);

        void Cancelar(int id);

        void Pagar(int id);

        bool IsLancamentoDuplicado(VerificarLancamentoDuplicadoDTO dto);
    }
}
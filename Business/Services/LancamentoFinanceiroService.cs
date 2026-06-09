namespace Business.Services
{
    using Business.Validators;
    using Data.Core.DTOs;
    using Data.Repositories;

    public class LancamentoFinanceiroService
    {
        private static readonly LancamentoFinanceiroRepository Repository = new();
        private static readonly CadastroLancamentoFinanceiroValidator CadastroValidator = new(Repository);
        private static readonly EdicaoLancamentoFinanceiroValidator EdicaoValidator = new(Repository);

        public LancamentoFinanceiroResponseDTO CadastrarLancamento(CriarLancamentoFinanceiroDTO dto)
        {
            CadastroValidator.Validar(dto);
            return LancamentoFinanceiroResponseDTO.FromEntity(Repository.Save(dto));
        }

        public LancamentoFinanceiroResponseDTO EditarLancamento(EditarLancamentoFinanceiroDTO dto)
        {
            EdicaoValidator.Validar(dto);
            return LancamentoFinanceiroResponseDTO.FromEntity(Repository.Update(dto));
        }
    }
}
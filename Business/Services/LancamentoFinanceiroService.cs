namespace Business.Services
{
    using Business.Validators;
    using Data.Core.DTOs;
    using Data.Models.DTOs;
    using Data.Repositories;

    public class LancamentoFinanceiroService
    {
        private static readonly LancamentoFinanceiroRepository Repository = new();
        private static readonly CadastroLancamentoFinanceiroValidator CadastroValidator = new(Repository);

        public LancamentoFinanceiroResponseDTO CadastrarLancamento(CriarLancamentoFinanceiroDTO dto)
        {
            CadastroValidator.Validar(dto);
            return this.Persistir(dto);
        }

        private LancamentoFinanceiroResponseDTO Persistir(CriarLancamentoFinanceiroDTO dto)
        {
            var entity = Repository.Save(dto);
            return LancamentoFinanceiroResponseDTO.FromEntity(entity);
        }
    }
}
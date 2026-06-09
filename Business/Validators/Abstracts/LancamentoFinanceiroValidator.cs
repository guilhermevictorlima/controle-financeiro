namespace Business.Validators.Abstracts
{
    using Business.Exceptions;
    using Data.Repositories.Interfaces;

    internal abstract class LancamentoFinanceiroValidator(ILancamentoFinanceiroRepository Repository)
    {
        protected ILancamentoFinanceiroRepository Repository = Repository;

        protected void ExecutarValidacoes(Dictionary<string, bool> violacoes)
        {
            foreach (KeyValuePair<string, bool> violacao in violacoes)
                if (violacao.Value)
                    throw new LancamentoFinanceiroException(violacao.Key);
        }
    }
}
namespace Business.Exceptions
{
    public class LancamentoFinanceiroException : Exception
    {
        public LancamentoFinanceiroException(string message) : base(message) { }
        public LancamentoFinanceiroException(string message, Exception inner) : base(message, inner) { }
    }
}

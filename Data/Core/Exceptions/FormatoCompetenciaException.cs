namespace Data.Core.Exceptions
{
    public class FormatoCompetenciaException : Exception
    {
        public FormatoCompetenciaException(string message) : base(message) {}
        public FormatoCompetenciaException(string message, Exception inner) : base(message, inner) {}
    }
}

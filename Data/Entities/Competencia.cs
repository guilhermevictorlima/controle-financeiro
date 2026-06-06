using System.Text.RegularExpressions;

namespace Data.Entities
{
    public class Competencia
    {
        private readonly string regexPadraoCompetencia = @"^\d{4}-(0[1-9]|1[0-2])$";

        public Competencia(string id)
        {
            if (!Regex.IsMatch(id, this.regexPadraoCompetencia))
            {
                throw new ArgumentException("A competência deve estar no formato YYYY-MM.", nameof(id));
            }

            this.Id = id;
        }

        required public string Id { get; init; }

    }
}

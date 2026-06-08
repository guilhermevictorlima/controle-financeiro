namespace Data.Core.ValueObjects
{
    using System.Text.RegularExpressions;
    using Data.Core.Exceptions;

    public readonly record struct Competencia
    {
        private static readonly string RegexPadraoCompetencia = @"^\d{4}-(0[1-9]|1[0-2])$";

        public string AnoMes { get; }

        public Competencia(string anoMes)
        {
            if (!Regex.IsMatch(anoMes, RegexPadraoCompetencia))
            {
                throw new FormatoCompetenciaException("A competência deve estar no formato YYYY-MM.");
            }

            this.AnoMes = anoMes;
        }

        public override string ToString() => this.AnoMes;
    }
}

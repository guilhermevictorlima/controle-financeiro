namespace Data.Models.Validators
{
    using System;
    using System.Text.RegularExpressions;

    internal static class CompetenciaValidator
    {
        private static readonly string RegexPadraoCompetencia = @"^\d{4}-(0[1-9]|1[0-2])$";

        public static void Validate(string competencia)
        {
            if (!Regex.IsMatch(competencia, RegexPadraoCompetencia))
            {
                throw new ArgumentException("A competência deve estar no formato YYYY-MM.");
            }
        }
    }
}

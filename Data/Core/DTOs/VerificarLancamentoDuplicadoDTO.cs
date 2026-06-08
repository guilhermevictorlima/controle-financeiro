namespace Data.Core.DTOs
{
    using Data.Core.ValueObjects;
    using Data.Models.Enums;

    public record VerificarLancamentoDuplicadoDTO(
        string Descricao,
        TipoLancamento Tipo,
        Competencia Competencia) {}
}

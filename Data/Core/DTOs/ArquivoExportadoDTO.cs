namespace Data.Core.DTOs
{
    public record ArquivoExportadoDTO(
        byte[] Conteudo,
        string NomeArquivo,
        string ContentType);
}

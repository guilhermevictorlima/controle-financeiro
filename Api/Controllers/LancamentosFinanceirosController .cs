using Business.Services;
using Data.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/lancamentos-financeiros")]
    public class LancamentosFinanceirosController : ControllerBase
    {
        private static readonly LancamentoFinanceiroService Service = new ();

        [HttpGet("listar")]
        public IActionResult Listar()
        {
            return Ok(Service.Listar());
        }

        [HttpGet("exportar")]
        public IActionResult Exportar([FromQuery] ExportarLancamentosDTO dto)
        {
            var arquivo = Service.ExportarLancamentos(dto);

            return File(
                arquivo.Conteudo,
                arquivo.ContentType,
                arquivo.NomeArquivo);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] CriarLancamentoFinanceiroDTO dto)
        {
            Service.CadastrarLancamento(dto);
            return Ok();
        }

        [HttpPut]
        public IActionResult Editar([FromBody] EditarLancamentoFinanceiroDTO dto)
        {
            Service.EditarLancamento(dto);
            return Ok();
        }

        [HttpPatch("{id:int}/pagar")]
        public IActionResult Pagar(int id)
        {
            Service.PagarLancamento(id);
            return Ok();
        }

        [HttpPatch("{id:int}/cancelar")]
        public IActionResult Cancelar(int id)
        {
            Service.CancelarLancamento(id);
            return Ok();
        }
    }
}

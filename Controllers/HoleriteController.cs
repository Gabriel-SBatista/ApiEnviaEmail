using APIEnviaEmail.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIEnviaEmail.Controllers;

[ApiController]
[Route("holerites")]
public class HoleriteController : ControllerBase
{
    private readonly HoleriteService _holeriteService;

    public HoleriteController(HoleriteService holeriteService)
    {
        _holeriteService = holeriteService;
    }

    [HttpPost]
    public async Task<ActionResult> Upload([FromForm] ICollection<IFormFile> files)
    {
        if (files.Count == 0 || files == null)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        await _holeriteService.EnviaHolerites(files);
        return Ok("Arquivos enviados com sucesso!");
    }
}

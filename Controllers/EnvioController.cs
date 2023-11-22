using APIEnviaEmail.Models;
using APIEnviaEmail.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIEnviaEmail.Controllers;

[ApiController]
[Route("envios")]
public class EnvioController : ControllerBase
{
    private readonly EnvioService _envioService;

    public EnvioController(EnvioService envioService)
    {
        _envioService = envioService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Envio>>> Get()
    {
        var envios = await _envioService.BuscaEnvios();
        return Ok(envios);
    }
}

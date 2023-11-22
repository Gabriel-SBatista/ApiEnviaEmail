using APIEnviaEmail.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIEnviaEmail.Controllers;

[ApiController]
[Route("holerites")]
public class HoleriteController : ControllerBase
{
    private readonly HoleriteService _holeriteService;
    private readonly EnvioService _envioService;
    private readonly StorageService _storageService;
    private readonly FuncionarioService _funcionarioService;

    public HoleriteController(HoleriteService holeriteService, EnvioService envioService, StorageService storageService, FuncionarioService funcionarioService)
    {
        _holeriteService = holeriteService;
        _envioService = envioService;
        _storageService = storageService;
        _funcionarioService = funcionarioService;
    }

    [HttpPost]
    public async Task<ActionResult> EnviaHolerite([FromForm] ICollection<IFormFile> files)
    {
        if (files.Count == 0 || files == null)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        await _holeriteService.PercorreZipEnviaHolerites(files);
        await _holeriteService.EnviaHoleritesStorage(files);
        return Ok("Arquivos enviados com sucesso!");
    }

    [HttpPost("reenvia")]
    public async Task<ActionResult> ReenviaHolerite(Guid id)
    {
        var envio = _envioService.BuscaEnvio(id);

        if(envio is null)
        {
            return NotFound("Envio não encontrado");
        }

        var funcionarioID = _holeriteService.BuscaIdFuncionario(envio.Nome);
        var funcionario = _funcionarioService.BuscaFuncionario(funcionarioID);
        var mes = _holeriteService.BuscaMes(envio.Nome);
        var tipoHolerite = _holeriteService.TipoHolerite(envio.Nome);

        var stream = await _storageService.DownloadArquivoAsync(envio.Nome);
        
        await _holeriteService.EnviaEmail(tipoHolerite, mes, funcionario, stream, envio.Nome);

        return Ok("Email reenviado com sucesso!!");
    }
}

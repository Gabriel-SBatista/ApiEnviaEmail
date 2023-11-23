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
        return Ok("Arquivos enviados com sucesso!");
    }

    [HttpPost("reenvia")]
    public async Task<ActionResult> ReenviaHolerite(Guid id)
    {
        var envio = await _envioService.BuscaEnvio(id);

        if(envio is null)
        {
            return NotFound("Envio não encontrado");
        }

        var vetorNome = _holeriteService.SeparaHoleriteNome(envio.Nome);
        var funcionario = await _funcionarioService.BuscaFuncionario(int.Parse(vetorNome[0]));
        var mes = _holeriteService.BuscaMes(int.Parse(vetorNome[1]));

        var stream = await _storageService.DownloadArquivoAsync(envio.Nome);
        
        await _holeriteService.EnviaEmail(vetorNome[2], mes, funcionario, stream, envio.Nome);

        return Ok("Email reenviado com sucesso!!");
    }
}

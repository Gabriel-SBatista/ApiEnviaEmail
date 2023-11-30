using APIEnviaEmail.Repositories;
using APIEnviaEmail.Services;
using APIEnviaEmail.Utils;
using Microsoft.AspNetCore.Mvc;

namespace APIEnviaEmail.Controllers;

[ApiController]
[Route("holerites")]
public class HoleriteController : ControllerBase
{
    private readonly HoleriteService _holeriteService;
    private readonly EnvioService _envioService;
    private readonly StorageService _storageService;
    private readonly EmailService _emailService;
    private readonly FuncionarioService _funcionarioService;

    public HoleriteController(HoleriteService holeriteService, EnvioService envioService, StorageService storageService, EmailService emailService, FuncionarioService funcionarioService)
    {
        _holeriteService = holeriteService;
        _envioService = envioService;
        _storageService = storageService;
        _emailService = emailService;
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

        var informacoesHolerite = StringUtils.ExtraiInformacoesHolerite(envio.Nome);     
        var stream = await _storageService.DownloadArquivoAsync(envio.Nome);
        var funcionario = await _funcionarioService.BuscarPorCodigo(informacoesHolerite.FuncionarioId);
        var email = _emailService.EscreveEmail(funcionario.Nome, informacoesHolerite.TipoHolerite, informacoesHolerite.Mes);

        if(await _emailService.EnviarEmailAsync(funcionario.Email, email, stream))
        {
            await _envioService.SalvaEnvio(envio.Nome, envio.HoleriteUrl, true, funcionario.FuncionarioId);

            return Ok("Email reenviado com sucesso!!");
        }

        await _envioService.SalvaEnvio(envio.Nome, envio.HoleriteUrl, false, funcionario.FuncionarioId);
        return BadRequest("Nao foi possivel reenviar o email");
    }
}

using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using APIEnviaEmail.Repositories;
using APIEnviaEmail.Utils;
using System.IO.Compression;

namespace APIEnviaEmail.Services;

public class HoleriteService
{
    private readonly EmailService _emailService;
    private readonly StorageService _storageService;
    private readonly FuncionarioService _funcionarioService;
    private readonly EnvioService _envioService;

    public HoleriteService(EmailService emailService, StorageService storageService, FuncionarioService funcionarioService, EnvioService envioService)
    {
        _emailService = emailService;
        _storageService = storageService;
        _funcionarioService = funcionarioService;
        _envioService = envioService;
    }  

    public async Task PercorreZipEnviaHolerites(ICollection<IFormFile> files)
    {
        foreach (var file in files)
        {
            using (var stream = file.OpenReadStream())
            using (var arquivoZip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entrada in arquivoZip.Entries)
                {
                    var informacoesHolerite = StringUtils.ExtraiInformacoesHolerite(entrada.Name);
                    var funcionario = await _funcionarioService.BuscarPorCodigo(informacoesHolerite.FuncionarioId);
                    var email = _emailService.EscreveEmail(funcionario.Nome, informacoesHolerite.TipoHolerite, informacoesHolerite.Mes);
                    var streamEnviaEmail = entrada.Open();
                    bool sucesso = false;

                    if(funcionario != null)
                    {
                        if(await _emailService.EnviarEmailAsync(funcionario.Email, email, streamEnviaEmail))
                        {
                            sucesso = true;
                        }
                    }                 
                    
                    var streamEnviaStorage = entrada.Open();

                    await _storageService.UploadArquivoAsync(entrada.Name, streamEnviaStorage);
                    string url = _storageService.GetUrl(entrada.Name);

                    await _envioService.SalvaEnvio(entrada.Name, url, sucesso, funcionario.FuncionarioId);
                }
            }
        }
    }
}

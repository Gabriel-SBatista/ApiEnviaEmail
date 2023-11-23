using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using System.IO.Compression;

namespace APIEnviaEmail.Services;

public class HoleriteService
{
    private readonly FuncionarioService _funcionarioService;
    private readonly EmailService _emailService;
    private readonly StorageService _storageService;
    private readonly EnvioService _envioService;
    private readonly IConfiguration _configuration;

    public HoleriteService(FuncionarioService funcionarioService, EmailService emailService, StorageService storageService, EnvioService envioService, IConfiguration configuration)
    {
        _funcionarioService = funcionarioService;
        _emailService = emailService;
        _storageService = storageService;
        _envioService = envioService;
        _configuration = configuration;
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
                    var vetorTitulo = SeparaHoleriteNome(entrada.Name);
                    var funcionario = await _funcionarioService.BuscaFuncionario(int.Parse(vetorTitulo[0]));
                    var mes = BuscaMes(int.Parse(vetorTitulo[1]));
                    var streamEnviaEmail = entrada.Open();

                    await EnviaEmail(vetorTitulo[2], mes, funcionario, streamEnviaEmail, entrada.Name);

                    var streamEnviaStorage = entrada.Open();

                    await _storageService.UploadArquivoAsync(entrada.Name, streamEnviaStorage);
                }
            }
        }
    }

    public async Task EnviaEmail(string tipoHolerite, string mes, Funcionario funcionario, Stream streamEnvia, string nomeArquivo)
    {
        if (tipoHolerite == "A")
            tipoHolerite = "Adiantamento";
        else
            tipoHolerite = "Pagamento";

        Envio envio = new Envio();

        string assunto = $"Recibo de {tipoHolerite} do mês de {mes}";
        string corpo = $"Olá, {funcionario.Nome}\nSegue o seu recibo de {tipoHolerite} do mês de {mes} em anexo.\nAbraços,\nRH";

        if (funcionario is not null)
        {
            var sucesso = await _emailService.EnviarEmailAsync(funcionario.Email, assunto, corpo, streamEnvia);

            if (sucesso == true)
                envio.Sucesso = true;

            envio.Funcionario = funcionario.CodigoFuncionario;
        }

        var url = _configuration.GetSection("AWSS3").GetRequiredSection("DefaultUrl").Value;
        envio.DataEnvio = DateTime.Now;
        envio.Nome = nomeArquivo;
        envio.HoleriteUrl = $"{url}{nomeArquivo}";
        await _envioService.SalvaEnvio(envio);
    }

    public string BuscaMes(int mes)
    {
        DateTime data = new DateTime(1111, mes, 11);

        string mesNome = data.ToString("MMMM");

        return mesNome;
    }

    public string[] SeparaHoleriteNome(string nome)
    {
        string[] vetorTitulo = nome.Split('_');
        return vetorTitulo;
    }
}

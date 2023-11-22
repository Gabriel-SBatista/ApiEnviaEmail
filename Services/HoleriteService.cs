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
                    var funcionarioID = BuscaIdFuncionario(entrada.Name);
                    var funcionario = await _funcionarioService.BuscaFuncionario(funcionarioID);
                    var mes = BuscaMes(entrada.Name);
                    var tipoHolerite = TipoHolerite(entrada.Name);
                    var streamEnvia = entrada.Open();

                    await EnviaEmail(tipoHolerite, mes, funcionario, streamEnvia, entrada.Name);                                                                                           
                }
            }
        }
    }

    public async Task EnviaEmail(string tipoHolerite, string mes, Funcionario funcionario, Stream streamEnvia, string nomeArquivo)
    {
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

    public async Task EnviaHoleritesStorage(ICollection<IFormFile> files)
    {
        foreach (var file in files)
        {
            using (var stream = file.OpenReadStream())
            using (var arquivoZip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entrada in arquivoZip.Entries)
                {
                    var streamUpload = entrada.Open();
                    await _storageService.UploadArquivoAsync(entrada.Name, streamUpload);
                }
            }
        }
    }

    public int BuscaIdFuncionario(string nome)
    {
        string stringFuncionarioID = "";

        foreach (var c in nome)
        {
            if (c.ToString() == "_")
            {
                break;
            }

            stringFuncionarioID += c.ToString();
        }

        int funcionarioID = int.Parse(stringFuncionarioID);

        return funcionarioID;
    }

    public string TipoHolerite(string nome)
    {
        string tipoHolerite = "";
        foreach (var c in nome)
        {
            if (c.ToString() == "A")
            {
                tipoHolerite = "Adiantamento";
            }
            else if (c.ToString() == "P")
            {
                tipoHolerite = "Pagamento";
            }
        }

        return tipoHolerite;
    }

    public string BuscaMes(string nome)
    {
        int i = 0;
        string mes = "";

        foreach (var c in nome)
        {
            if (i == 1 && c.ToString() != "_")
            {
                mes += c.ToString();
            }

            if (c.ToString() == "_")
            {
                i++;
            }
        }

        switch(mes)
        {
            case "1":
                mes = "Janeiro";
                break;
            case "2":
                mes = "Fevereiro";
                break;
            case "3":
                mes = "Março";
                break;
            case "4":
                mes = "Abril";
                break;
            case "5":
                mes = "Maio";
                break;
            case "6":
                mes = "Junho";
                break;
            case "7":
                mes = "Julho";
                break;
            case "8":
                mes = "Agosto";
                break;
            case "9":
                mes = "Setembro";
                break;
            case "10":
                mes = "Outubro";
                break;
            case "11":
                mes = "Novembro";
                break;
            case "12":
                mes = "Dezembro";
                break;
        }

        return mes;
    }
}

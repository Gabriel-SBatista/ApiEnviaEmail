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

    public HoleriteService(FuncionarioService funcionarioService, EmailService emailService, StorageService storageService, EnvioService envioService)
    {
        _funcionarioService = funcionarioService;
        _emailService = emailService;
        _storageService = storageService;
        _envioService = envioService;
    }  

    public async Task EnviaHolerites(ICollection<IFormFile> files)
    {
        foreach (var file in files)
        {
            using (var stream = file.OpenReadStream())
            using (var arquivoZip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entrada in arquivoZip.Entries)
                {
                    var funcionarioID = BuscaIdFuncionario(entrada);
                    var funcionario = _funcionarioService.BuscaFuncionario(funcionarioID);
                    var mes = BuscaMes(entrada);
                    var tipoHolerite = TipoHolerite(entrada);                
                    Envio envio = new Envio();

                    if (funcionario is not null)
                    {
                        string assunto = $"Recibo de {tipoHolerite} do mês de {mes}";
                        string corpo = $"Olá, {funcionario.Nome}\nSegue o seu recibo de {tipoHolerite} do mês de {mes} em anexo.\nAbraços,\nRH";
                        var streamEnvia = entrada.Open();
                        await _emailService.EnviarEmailAsync(funcionario.Email, assunto, corpo, streamEnvia).ContinueWith(task =>
                        {
                            if (task.Status == TaskStatus.RanToCompletion)
                                envio.Sucesso = true;
                        });
                        envio.Funcionario = funcionario.CodigoFuncionario;
                    }

                    var streamUpload = entrada.Open();
                    await _storageService.UploadArquivoAsync(entrada.Name, streamUpload);
                                      
                    envio.DataEnvio = DateTime.Now;
                    envio.Nome = entrada.Name;
                    envio.HoleriteUrl = $"https://gabrielsb-bucket.s3.sa-east-1.amazonaws.com/{entrada.Name}";
                    _envioService.SalvaEnvio(envio);
                }
            }
        }
    }

    public static int BuscaIdFuncionario(ZipArchiveEntry entrada)
    {
        string stringFuncionarioID = "";

        foreach (var c in entrada.Name)
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

    public static string TipoHolerite(ZipArchiveEntry entrada)
    {
        string tipoHolerite = "";
        foreach (var c in entrada.Name)
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

    public static string BuscaMes(ZipArchiveEntry entrada)
    {
        int i = 0;
        string mes = "";

        foreach (var c in entrada.Name)
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

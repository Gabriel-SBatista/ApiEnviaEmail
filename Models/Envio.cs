using System.ComponentModel.DataAnnotations;

namespace APIEnviaEmail.Models;

public class Envio
{
    public Envio()
    {
        Sucesso = false;
        Funcionario = null;
    }

    public Envio(string nome, string url, bool sucesso, int funcionario, DateTime data)
    {
        Nome = nome;
        HoleriteUrl = url;
        Funcionario = funcionario;
        Sucesso= sucesso;
        DataEnvio = data;
    }

    public Guid EnvioId { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    public string HoleriteUrl { get; set; }
    [Required]
    public DateTime DataEnvio { get; set; }
    [Required]
    public bool Sucesso { get; set; }
    public int? Funcionario { get; set; }
}

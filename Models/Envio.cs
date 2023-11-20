using System.ComponentModel.DataAnnotations;

namespace APIEnviaEmail.Models;

public class Envio
{
    public Envio()
    {
        Sucesso = false;
        Funcionario = null;
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

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APIEnviaEmail.Models;

public class Funcionario
{
    public int FuncionarioId { get; set; }
    [Required]
    [MaxLength(20)]
    public string Nome { get; set; }
    [Required]
    [MaxLength(30)]
    public string Email { get; set; }
    [Required]
    public int CodigoFuncionario { get; set; }
}

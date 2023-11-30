namespace APIEnviaEmail.Models;

public class IdentificaHolerite
{
   
    public int FuncionarioId { get; set; }
    public string Mes { get; set; }
    public string TipoHolerite { get; set; }

    public IdentificaHolerite(int funcionario, string mes, string tipoHolerite)
    {
        FuncionarioId = funcionario;
        Mes = mes;
        TipoHolerite = tipoHolerite;
    }
}

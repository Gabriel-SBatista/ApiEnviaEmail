using APIEnviaEmail.Models;

namespace APIEnviaEmail.Utils;

public class StringUtils
{
    public static IdentificaHolerite ExtraiInformacoesHolerite(string nome)
    {
        string[] vetorTitulo = nome.Split('_');
        var funcionarioId = int.Parse(vetorTitulo[0]);
        var mes = MesUtils.BuscaMes(int.Parse(vetorTitulo[1]));
        string tipoHolerite;

        if (vetorTitulo[2] == "A")
            tipoHolerite = "Adiantamento";
        else
            tipoHolerite = "Pagamento";

        var holerite = new IdentificaHolerite(funcionarioId, mes, tipoHolerite);

        return holerite;
    }
}

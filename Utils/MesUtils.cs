namespace APIEnviaEmail.Utils;

public class MesUtils
{
    public static string BuscaMes(int mes)
    {
        DateTime data = new DateTime(1111, mes, 11);

        string mesNome = data.ToString("MMMM");

        return mesNome;
    }
}

using APIEnviaEmail.Context;
using APIEnviaEmail.Models;

namespace APIEnviaEmail.Services;

public class EnvioService
{
    private readonly AppDbContext _context;

    public EnvioService(AppDbContext context)
    {
        _context = context;
    }

    public void SalvaEnvio(Envio envio)
    {
        _context.Envios.Add(envio);
        _context.SaveChanges();
    }

    public List<Envio> BuscaEnvios()
    {
        var envios = _context.Envios.ToList();
        return envios;
    }
}

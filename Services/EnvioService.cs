using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Services;

public class EnvioService
{
    private readonly AppDbContext _context;

    public EnvioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SalvaEnvio(Envio envio)
    {
        await _context.Envios.AddAsync(envio);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Envio>> BuscaEnvios()
    {
        var envios = await _context.Envios.ToListAsync();
        return envios;
    }

    public async Task<Envio> BuscaEnvio(Guid id)
    {
        var envio = await _context.Envios.FindAsync(id);
        return envio;
    }
}

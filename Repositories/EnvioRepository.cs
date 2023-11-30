using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Repositories;

public class EnvioRepository : IEnvioRepository
{
    private readonly AppDbContext _context;

    public EnvioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Criar(Envio envio)
    {
        await _context.Envios.AddAsync(envio);
        await _context.SaveChangesAsync();
    }

    public async Task<Envio> BuscarPorId(Guid id)
    {
        var envio = await _context.Envios.FindAsync(id);
        return envio;
    }

    public async Task<List<Envio>> Buscar()
    {
        var envios = await _context.Envios.ToListAsync();
        return envios;
    }
}

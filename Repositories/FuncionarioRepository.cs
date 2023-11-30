using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Repositories;

public class FuncionarioRepository : IFuncionarioRepository
{
    private readonly AppDbContext _context;

    public FuncionarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Criar(Funcionario funcionario)
    {
        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task<Funcionario> BuscarPorCodigoFuncionario(int cf)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.CodigoFuncionario == cf);

        return funcionario;
    }

    public async Task<List<Funcionario>> Buscar()
    {
        var funcionarios = await _context.Funcionarios.ToListAsync();

        return funcionarios;
    }

    public async Task Excluir(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        _context.Remove(funcionario);
        await _context.SaveChangesAsync();
    }
}

using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Services;

public class FuncionarioService
{
    private readonly AppDbContext _context;

    public FuncionarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CadastraFuncionario(Funcionario funcionario)
    {
        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task<Funcionario?> BuscaFuncionario(int id)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.CodigoFuncionario == id);

        return funcionario;
    }

    public async Task<List<Funcionario>> BuscaFuncionarios()
    {
        var funcionarios = await _context.Funcionarios.ToListAsync();

        return funcionarios;
    }

    public async Task DeletaFuncionario(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        _context.Remove(funcionario);
        await _context.SaveChangesAsync();
    }
}

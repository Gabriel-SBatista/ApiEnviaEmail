using APIEnviaEmail.Context;
using APIEnviaEmail.Models;

namespace APIEnviaEmail.Services;

public class FuncionarioService
{
    private readonly AppDbContext _context;

    public FuncionarioService(AppDbContext context)
    {
        _context = context;
    }

    public void CadastraFuncionario(Funcionario funcionario)
    {
        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges();
    }

    public Funcionario? BuscaFuncionario(int id)
    {
        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.CodigoFuncionario == id);

        return funcionario;
    }

    public List<Funcionario> BuscaFuncionarios()
    {
        var funcionarios = _context.Funcionarios.ToList();

        return funcionarios;
    }

    public void DeletaFuncionario(int id)
    {
        var funcionario = _context.Funcionarios.Find(id);

        _context.Remove(funcionario);
        _context.SaveChanges();
    }
}

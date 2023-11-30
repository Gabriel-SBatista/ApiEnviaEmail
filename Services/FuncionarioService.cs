using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using APIEnviaEmail.Repositories;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Services;

public class FuncionarioService
{
    private readonly IFuncionarioRepository _repository;

    public FuncionarioService(IFuncionarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<Funcionario> BuscarPorCodigo(int cf)
    {
        var funcionario = await _repository.BuscarPorCodigoFuncionario(cf); 
        return funcionario;
    }

    public async Task CadastraFuncionario(Funcionario funcionario)
    {
        await _repository.Criar(funcionario);
    }

    public async Task<List<Funcionario>> BuscaFuncionarios()
    {
        var funcionarios = await _repository.Buscar();
        return funcionarios;
    }

    public async Task DeletaFuncionario(int id)
    {
        await _repository.Excluir(id);
    }
}

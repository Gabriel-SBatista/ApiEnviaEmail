using APIEnviaEmail.Models;

namespace APIEnviaEmail.Repositories
{
    public interface IFuncionarioRepository
    {
        Task<List<Funcionario>> Buscar();
        Task<Funcionario> BuscarPorCodigoFuncionario(int cf);
        Task Criar(Funcionario funcionario);
        Task Excluir(int id);
    }
}
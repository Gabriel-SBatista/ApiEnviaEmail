using APIEnviaEmail.Models;

namespace APIEnviaEmail.Repositories
{
    public interface IEnvioRepository
    {
        Task Criar(Envio envio);
        Task<Envio> BuscarPorId(Guid id);
        Task<List<Envio>> Buscar();
    }
}
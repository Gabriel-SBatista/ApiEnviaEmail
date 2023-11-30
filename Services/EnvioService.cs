using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using APIEnviaEmail.Repositories;
using Microsoft.EntityFrameworkCore;

namespace APIEnviaEmail.Services;

public class EnvioService
{
    private readonly IEnvioRepository _repository;

    public EnvioService(IEnvioRepository repository)
    {
        _repository = repository;
    }

    public async Task SalvaEnvio(string nomeArquivo, string url, bool sucesso, int funcionarioId)
    {
        Envio envio = new Envio();

        envio.DataEnvio = DateTime.Now;
        envio.Nome = nomeArquivo;
        envio.HoleriteUrl = url;
        envio.Sucesso = sucesso;
        envio.Funcionario = funcionarioId;

        await _repository.Criar(envio);
    }

    public async Task<List<Envio>> BuscaEnvios()
    {
        var envios = await _repository.Buscar();
        return envios;
    }

    public async Task<Envio> BuscaEnvio(Guid id)
    {
        var envio = await _repository.BuscarPorId(id);
        return envio;
    }
}

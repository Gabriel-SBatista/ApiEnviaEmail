using APIEnviaEmail.Context;
using APIEnviaEmail.Models;
using APIEnviaEmail.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIEnviaEmail.Controllers;

[ApiController]
[Route("funcionarios")]
public class FuncionarioController : ControllerBase
{
    private readonly FuncionarioService _services;

    public FuncionarioController(FuncionarioService services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var funcionarios = await _services.BuscaFuncionarios();

        if(funcionarios is null)
        {
            return NotFound();
        }

        return Ok(funcionarios);
    }

    [HttpPost]
    public async Task<ActionResult> Post(Funcionario funcionario)
    {
        await _services.CadastraFuncionario(funcionario);
        return Ok(funcionario);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        await _services.DeletaFuncionario(id);
        return Ok("Funcionario excluido com sucesso!!");
    }
}

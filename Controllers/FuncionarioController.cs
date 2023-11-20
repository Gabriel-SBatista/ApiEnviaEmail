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
    public ActionResult<ICollection<Funcionario>> Get()
    {
        var funcionarios = _services.BuscaFuncionarios();

        if(funcionarios is null)
        {
            return NotFound();
        }

        return funcionarios;
    }

    [HttpPost]
    public ActionResult Post(Funcionario funcionario)
    {
        _services.CadastraFuncionario(funcionario);
        return Ok(funcionario);
    }

    [HttpDelete]
    public ActionResult Delete(int id)
    {
        _services.DeletaFuncionario(id);
        return Ok("Funcionario excluido com sucesso!!");
    }
}

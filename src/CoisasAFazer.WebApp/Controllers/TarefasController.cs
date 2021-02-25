using CoisasAFazer.Core.Commands;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoisasAFazer.WebApp.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class TarefasController : ControllerBase
  {
    IRepositorioTarefas _repo;
    ILogger<CadastraTarefaHandler> _logger;


    public TarefasController(IRepositorioTarefas repo, ILogger<CadastraTarefaHandler> logger)
    {
      _repo = repo;
      _logger = logger;
    }

    [HttpPost]
    public IActionResult EndpointCadastraTarefa(CadastraTarefaVM model)
    {
      var cmdObtemCateg = new ObtemCategoriaPorId(model.IdCategoria);
      var categoria = new ObtemCategoriaPorIdHandler(_repo).Execute(cmdObtemCateg);
      if (categoria == null)
      {
        return NotFound("Categoria não encontrada");
      }
      var comando = new CadastraTarefa(model.Titulo, categoria, model.Prazo);
      var handler = new CadastraTarefaHandler(_repo, _logger);
      var resultado = handler.Execute(comando);
      if (resultado.IsSuccess) return Ok();
      return StatusCode(500);
    }

  }
}
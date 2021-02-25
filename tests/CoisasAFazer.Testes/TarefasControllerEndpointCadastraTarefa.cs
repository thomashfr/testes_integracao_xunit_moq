using System;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using CoisasAFazer.WebApp.Controllers;
using CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoisasAFazer.Testes
{
  public class TarefasControllerEndpointCadastraTarefa
  {
    [Fact]
    public void DataTarefaComInformaçõesValidasRetornar200()
    {
      //Given
      var model = new CadastraTarefaVM();

      model.IdCategoria = 20;
      model.Titulo = "Estudar Xunit";
      model.Prazo = new DateTime(2019, 12, 31);


      var options = new DbContextOptionsBuilder<DbTarefasContext>()
      .UseInMemoryDatabase("DbTarefasContex")
      .Options;
      var context = new DbTarefasContext(options);
      context.Categorias.Add(new Categoria(20, "Estudo"));
      context.SaveChanges();
      var repo = new RepositorioTarefa(context);

      var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

      var controlador = new TarefasController(repo, mockLogger.Object);
      //When
      var retorno = controlador.EndpointCadastraTarefa(model);

      //Then
      Assert.IsType<OkResult>(retorno);

    }
    [Fact]
    public void QuandoExcecaoforLancadaDeveRetornarStatusCode500()
    {
      //Given
      var model = new CadastraTarefaVM();

      model.IdCategoria = 20;
      model.Titulo = "Estudar Xunit";
      model.Prazo = new DateTime(2019, 12, 31);




      var mock = new Mock<IRepositorioTarefas>();
      mock.Setup(r => r.ObtemCategoriaPorId(20)).Returns(new Categoria(20, "Estudo"));
      mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
      .Throws(new Exception("Houve um erro"));

      var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

      var controlador = new TarefasController(mock.Object, mockLogger.Object);
      //When
      var retorno = controlador.EndpointCadastraTarefa(model);

      //Then
      Assert.IsType<StatusCodeResult>(retorno);
      var statusCode = (retorno as StatusCodeResult).StatusCode;
      Assert.Equal(500, statusCode);

    }
  }
}
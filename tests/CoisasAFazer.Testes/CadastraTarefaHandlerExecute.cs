using System;
using System.Linq;
using System.Runtime.InteropServices;
using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CoisasAFazer.Testes
{
  public class CadastraTarefaHandlerExecute
  {
    [Fact]
    public void DadaTarefaComInformacoesValidasDeveIncluirNoBD()
    {
      //Given
      var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

      var mock = new Mock<ILogger<CadastraTarefaHandler>>();

      var options = new DbContextOptionsBuilder<DbTarefasContext>()
      .UseInMemoryDatabase("DbTarefasContex1")
      .Options;
      var context = new DbTarefasContext(options);
      var repo = new RepositorioTarefa(context);
      var handler = new CadastraTarefaHandler(repo, mock.Object);
      //When
      handler.Execute(comando);

      //Then
      var tarefas = repo.ObtemTarefas(t => t.Titulo == "Estudar Xunit").FirstOrDefault();
      Assert.NotNull(tarefas);


    }

    [Fact]
    public void QuandoExceptionForLancadaResultadoIsSuccessDeveSerFalse()
    {
      var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

      var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

      var mock = new Mock<IRepositorioTarefas>();
      mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
      .Throws(new Exception("Houve um erro na inclusão de tarefas"));

      var repo = mock.Object;

      var handler = new CadastraTarefaHandler(repo, mockLogger.Object);
      //When
      CommandResult resultado = handler.Execute(comando);

      //Then
      Assert.False(resultado.IsSuccess);


    }
    [Fact]
    public void QuandoExceptionForLancadaDeveLogarAMensagem()
    {
      var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));
      var mensagemDeErro = "Houve um erro na inclusão de tarefas";
      var execaoEsperada = new Exception(mensagemDeErro);
      var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

      var mock = new Mock<IRepositorioTarefas>();
      mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa>()))
      .Throws(execaoEsperada);

      var repo = mock.Object;
      var log = mockLogger.Object;

      var handler = new CadastraTarefaHandler(repo, log);
      //When
      CommandResult resultado = handler.Execute(comando);

      //Then
      // mockLogger.Verify(l => l.Log(
      //   LogLevel.Error, //nivel de log
      //    It.IsAny<EventId>(), //identificador do evento
      //     It.IsAny<object>(), //Objeto que será logado
      //      execaoEsperada, //excecao que sera logada
      //           It.IsAny<Func<object, Exception, string>>()), // função que converte objeto+exceção >> string
      //  Times.Once());

      Assert.Equal(LogLevel.Error, mockLogger.Invocations[1].Arguments[0]);
    }






    [Fact]
    public void DadaTarefaComInformacoesValidasDeveLogar()
    {
      var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

      var mock = new Mock<IRepositorioTarefas>();

      var repo = mock.Object;
      var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
      mockLogger.Setup(l => l.Log(
         It.IsAny<LogLevel>(), //nivel de log
          It.IsAny<EventId>(), //identificador do evento
          It.IsAny<object>(), //Objeto que será logadoDS
            It.IsAny<Exception>(), //excecao que sera logada
             It.IsAny<Func<object, Exception, string>>()));



      var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

      handler.Execute(comando);

      //Then
      Assert.Equal(LogLevel.Debug, mockLogger.Invocations[0].Arguments[0]);
      Assert.Contains("Estudar Xunit", mockLogger.Invocations[0].Arguments[2].ToString());
    }
  }
}

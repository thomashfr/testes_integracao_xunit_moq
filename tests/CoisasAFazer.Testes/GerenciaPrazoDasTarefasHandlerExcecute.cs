using System;
using System.Collections.Generic;
using System.Linq;
using CoisasAFazer.Core.Commands;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CoisasAFazer.Testes
{
  public class GerenciaPrazoDasTarefasHandlerExcecute
  {
    [Fact]
    public void QuandoTarefasEstiveremAtrasadasDeveMudarSeuStatus()
    {
      //Given
      var compCategoria = new Categoria(1, "Compras");
      var casaCategoria = new Categoria(2, "Casa");
      var trabalhoCategoria = new Categoria(3, "Trabalho");
      var saudeCategoria = new Categoria(4, "Saúde");
      var higieneCategoria = new Categoria(5, "Higiene");

      var tarefas = new List<Tarefa>
      {
          // atrasadas a partir de 01/01/2019
          new Tarefa(1, "Tirar o Lixo", casaCategoria, new DateTime(2018,12,31), null,StatusTarefa.Criada),
          new Tarefa(4, "Fazer Almoço", casaCategoria, new DateTime(2017,12,31), null,StatusTarefa.Criada),
          new Tarefa(9, "Ir a Academia", saudeCategoria, new DateTime(2018,12,31), null,StatusTarefa.Criada),
          new Tarefa(7, "Concluir o relatório", trabalhoCategoria, new DateTime(2018,05,7), null,StatusTarefa.Criada),
          new Tarefa(10, "Beber Agua", saudeCategoria, new DateTime(2018,12,31), null,StatusTarefa.Pendente),
          // dentro do prazo em 01/01/2019
          new Tarefa(8, "Comparecer a reunião", trabalhoCategoria, new DateTime(2018,11,12),new DateTime(2018,11,30),StatusTarefa.Concluida),
          new Tarefa(2, "Arrumar a cama", casaCategoria, new DateTime(2019,4,5), null,StatusTarefa.Criada),
          new Tarefa(3, "Escovar os dentes", higieneCategoria, new DateTime(2019,1,2), null,StatusTarefa.Criada),
          new Tarefa(5, "Comprar presente pro Jõao", compCategoria, new DateTime(2019,10,8), null,StatusTarefa.Criada),
          new Tarefa(6, "Comprar ração", compCategoria, new DateTime(2019,11,20), null,StatusTarefa.Criada),

      };

      var options = new DbContextOptionsBuilder<DbTarefasContext>()
      .UseInMemoryDatabase("DbTarefasContex")
      .Options;
      var context = new DbTarefasContext(options);
      var repo = new RepositorioTarefa(context);

      repo.IncluirTarefas(tarefas.ToArray());

      var comando = new GerenciaPrazoDasTarefas(new DateTime(2019, 1, 1));
      var handler = new GerenciaPrazoDasTarefasHandler(repo);

      //When

      handler.Execute(comando);

      //Then

      var tarefaEmAtraso = repo.ObtemTarefas(t => t.Status == StatusTarefa.EmAtraso);
      Assert.Equal(5, tarefaEmAtraso.Count());

    }

    [Fact]
    public void QuandoInvocadoDeveChamarAtualizarTarefasNaQtdadeVezesDoTotaldeTaredasAtrazadas()
    {
      //Given
      var tarefas = new List<Tarefa>
        {
         new Tarefa(1, "Tirar o Lixo", new Categoria("Dumy"), new DateTime(2018,12,31), null,StatusTarefa.Criada),
          new Tarefa(4, "Fazer Almoço", new Categoria("Dumy"), new DateTime(2017,12,31), null,StatusTarefa.Criada),
          new Tarefa(9, "Ir a Academia", new Categoria("Dumy"), new DateTime(2018,12,31), null,StatusTarefa.Criada),
        };
      var mock = new Mock<IRepositorioTarefas>();
      mock.Setup(r => r.ObtemTarefas(It.IsAny<Func<Tarefa, bool>>()))
      .Returns(tarefas);
      var repo = mock.Object;

      var comando = new GerenciaPrazoDasTarefas(new DateTime(2019, 1, 1));
      var handler = new GerenciaPrazoDasTarefasHandler(repo);

      //When

      handler.Execute(comando);


      //Then

      mock.Verify(r => r.AtualizarTarefas(It.IsAny<Tarefa[]>()), Times.Once());

    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using CoisasAFazer.Core.Models;
using CoisasAFazer.Infrastructure;

namespace CoisasAFazer.Testes
{
  public class RepositorioFake : IRepositorioTarefas
  {
    List<Tarefa> lista = new List<Tarefa>();
    public void AtualizarTarefas(params Tarefa[] tarefas)
    {
      throw new NotImplementedException();
    }

    public void ExcluirTarefas(params Tarefa[] tarefas)
    {
      throw new NotImplementedException();
    }

    public void IncluirTarefas(params Tarefa[] tarefas)
    {
      tarefas.ToList().ForEach(tarefa => lista.Add(tarefa));
    }

    public Categoria ObtemCategoriaPorId(int id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Tarefa> ObtemTarefas(Func<Tarefa, bool> filtro)
    {
      return lista.Where(filtro);
    }
  }
}
using System;

namespace CoisasAFazer.Core.Commands
{
  public class GerenciaPrazoDasTarefas
  {
    public DateTime DataHoraAtual { get; }
    public GerenciaPrazoDasTarefas(DateTime dataHoraAtual)
    {
      DataHoraAtual = dataHoraAtual;
    }
  }
}
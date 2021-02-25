using System;

namespace CoisasAFazer.Core.Models
{
  public class Tarefa
  {
    public Tarefa()
    {

    }

    public Tarefa(int id, string titulo, Categoria categoria, DateTime prazo, DateTime? concluidaEm, StatusTarefa status)
    {
      this.Id = id;
      this.Titulo = titulo;
      this.Categoria = categoria;
      this.Prazo = prazo;
      this.ConcluidaEm = concluidaEm;
      this.Status = status;
    }

    public int Id { get; set; }


    public string Titulo { get; set; }


    public Categoria Categoria { get; set; }


    public DateTime Prazo { get; set; }


    public DateTime? ConcluidaEm { get; set; }


    public StatusTarefa Status { get; set; }

    public override string ToString()
    {
      return $"{Id}, {Titulo}, {Categoria.Descricao}, {Prazo.ToString("dd/MM/yyyy")}";
    }

  }
}
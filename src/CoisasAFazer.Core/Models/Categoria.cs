namespace CoisasAFazer.Core.Models
{

  public class Categoria
  {
    public string Descricao { get; private set; }
    public int Id { get; private set; }
    public Categoria(string descricao)
    {
      Descricao = descricao;
    }
    public Categoria(int id, string descricao) : this(descricao)
    {
      Id = id;
    }

  }
}
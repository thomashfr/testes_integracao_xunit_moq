using CoisasAFazer.Core.Commands;
using CoisasAFazer.Infrastructure;
using CoisasAFazer.Services.Handlers;
using Moq;
using Xunit;

namespace CoisasAFazer.Testes
{
  public class ObtemCategoriaPorIdExecute
  {
    [Fact]
    public void QuandoIdForExistenteDeveChamarObtemCategotiaPorIdUmaUnicaVez()
    {
      //Given
      var idCategoria = 20;
      var comando = new ObtemCategoriaPorId(idCategoria);
      var mock = new Mock<IRepositorioTarefas>();
      var repo = mock.Object;

      var handler = new ObtemCategoriaPorIdHandler(repo);

      //When
      handler.Execute(comando);
      //Then
      mock.Verify(r => r.ObtemCategoriaPorId(idCategoria), Times.Once());
    }
  }
}
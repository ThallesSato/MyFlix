using Moq;
using MyFlix.Application.Dtos.Input;
using MyFlix.Application.Services;
using MyFlix.Domain.Models;
using MyFlix.Infra.Interfaces;

namespace MyFlix.Test.Application.Services;

public class FilmeServiceTests
{
    private readonly Mock<IFilmeRepository> _mockFilmeRepository;
    private readonly FilmeService _filmeService;

    public FilmeServiceTests()
    {
        _mockFilmeRepository = new Mock<IFilmeRepository>();
        _filmeService = new FilmeService(_mockFilmeRepository.Object);
    }

    [Fact]
    public async Task GetAllFilmesAsync_DeveRetornarListaDeFilmes()
    {
        // Arrange
        var filmesEsperados = new List<Filme>
        {
            new()
            { 
                Id = 1, 
                Titulo = "Filme 1",
                AnoLancamento = 2000,
                Genero = "Teste 1",
                Status = true,
                Nota = 5
            },
            new()
            { 
                Id = 2, 
                Titulo = "Filme 2",
                AnoLancamento = 2001,
                Genero = "Teste 2"
            }
        };

        _mockFilmeRepository
            .Setup(repo => repo.GetAllFilmesAsync())
            .ReturnsAsync(filmesEsperados);

        // Act
        var resultado = await _filmeService.GetAllFilmesAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(filmesEsperados.Count, resultado.Count);
        Assert.Equal(filmesEsperados, resultado);
        _mockFilmeRepository.Verify(repo => repo.GetAllFilmesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllFilmesAsync_QuandoListaVazia_DeveRetornarListaVazia()
    {
        // Arrange
        var listaVazia = new List<Filme>();
            
        _mockFilmeRepository
            .Setup(repo => repo.GetAllFilmesAsync())
            .ReturnsAsync(listaVazia);

        // Act
        var resultado = await _filmeService.GetAllFilmesAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        _mockFilmeRepository.Verify(repo => repo.GetAllFilmesAsync(), Times.Once);
    }

    [Fact]
    public async Task PostFilmeAsync_DeveInserirFilmeComSucesso()
    {
        // Arrange
        var filmeDto = new FilmeDto
        {
            Titulo = "Teste",
            AnoLancamento = 2010,
            Genero = "Teste"
        };

        _mockFilmeRepository
            .Setup(repo => repo.PostFilmeAsync(It.IsAny<Filme>()))
            .Returns(Task.CompletedTask);

        // Act
        await _filmeService.PostFilmeAsync(filmeDto);

        // Assert
        _mockFilmeRepository.Verify(repo => repo.PostFilmeAsync(
            It.Is<Filme>(f => 
                f.Titulo == filmeDto.Titulo &&
                f.AnoLancamento == filmeDto.AnoLancamento &&
                f.Genero == filmeDto.Genero 
            )), Times.Once);
    }

    [Fact]
    public async Task DeleteFilmeByIdAsync_QuandoFilmeExiste_DeveRetornarTrue()
    {
        // Arrange
        var id = 1;
        _mockFilmeRepository
            .Setup(repo => repo.DeleteFilmeByIdAsync(id))
            .ReturnsAsync(true);

        // Act
        var resultado = await _filmeService.DeleteFilmeByIdAsync(id);

        // Assert
        Assert.True(resultado);
        _mockFilmeRepository.Verify(repo => repo.DeleteFilmeByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteFilmeByIdAsync_QuandoFilmeNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        var id = 999;
        _mockFilmeRepository
            .Setup(repo => repo.DeleteFilmeByIdAsync(id))
            .ReturnsAsync(false);

        // Act
        var resultado = await _filmeService.DeleteFilmeByIdAsync(id);

        // Assert
        Assert.False(resultado);
        _mockFilmeRepository.Verify(repo => repo.DeleteFilmeByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task UpdateFilmeByIdAsync_QuandoFilmeExiste_DeveRetornarTrue()
    {
        // Arrange
        var id = 1;
        var filme = new Filme
        {
            Id = id,
            Titulo = "Filme",
            AnoLancamento = 2000,
            Genero = "teste",
            Status = true,
            Nota = 5
        };

        var filmeAtualizado = new Filme
        {
            Titulo = "Filme Atualizado",
            AnoLancamento = 2001,
            Genero = "teste",
            Status = false
        };

        _mockFilmeRepository
            .Setup(repo => repo.GetFilmeByIdAsyncOrNull(id))
            .ReturnsAsync(filme);

        _mockFilmeRepository
            .Setup(repo => repo.UpdateFilme(It.IsAny<Filme>()))
            .Verifiable();

        // Act
        var resultado = await _filmeService.UpdateFilmeByIdAsync(id, filmeAtualizado);

        // Assert
        Assert.True(resultado);
        _mockFilmeRepository.Verify(repo => repo.GetFilmeByIdAsyncOrNull(id), Times.Once);
        _mockFilmeRepository.Verify(repo => repo.UpdateFilme(It.Is<Filme>(f =>
            f.Id == id &&
            f.Titulo == filmeAtualizado.Titulo &&
            f.AnoLancamento == filmeAtualizado.AnoLancamento &&
            f.Genero == filmeAtualizado.Genero &&
            f.Status == filmeAtualizado.Status &&
            f.Nota == filmeAtualizado.Nota)), Times.Once);
    }

    [Fact]
    public async Task UpdateFilmeByIdAsync_QuandoFilmeNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        var id = 999;
        var filme = new Filme
        {
            Titulo = "Filme",
            AnoLancamento = 2001,
            Genero = "teste",
            Status = false
        };

        _mockFilmeRepository
            .Setup(repo => repo.GetFilmeByIdAsyncOrNull(id))
            .ReturnsAsync((Filme?)null);

        // Act
        var resultado = await _filmeService.UpdateFilmeByIdAsync(id, filme);

        // Assert
        Assert.False(resultado);
        _mockFilmeRepository.Verify(repo => repo.GetFilmeByIdAsyncOrNull(id), Times.Once);
        _mockFilmeRepository.Verify(repo => repo.UpdateFilme(It.IsAny<Filme>()), Times.Never);
    }
}
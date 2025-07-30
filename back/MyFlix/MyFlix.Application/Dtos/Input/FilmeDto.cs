namespace MyFlix.Application.Dtos.Input;

public class FilmeDto
{
    public string Titulo { get; set; } = String.Empty;

    public int AnoLancamento { get; set; }

    public string Genero { get; set; } = String.Empty;
}
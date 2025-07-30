namespace MyFlix.Domain.Models;

public class Filme
{
    public int Id { get; set; }

    public string Titulo { get; set; } = String.Empty;

    public int AnoLancamento { get; set; }

    public string Genero { get; set; } = String.Empty;

    public bool Status { get; set; } 

    public int? Nota { get; set; }
}
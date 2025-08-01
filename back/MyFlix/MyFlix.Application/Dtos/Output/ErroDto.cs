namespace MyFlix.Application.Dtos.Output;

public class ErroDto
{
    public string Mensagem { get; set; }
    public int StatusCode { get; set; }

    public ErroDto(string mensagem, int statusCode)
    {
        Mensagem = mensagem;
        StatusCode = statusCode;
    }
}
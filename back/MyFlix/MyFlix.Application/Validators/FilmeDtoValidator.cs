using FluentValidation;
using MyFlix.Application.Dtos.Input;

namespace MyFlix.Application.Validators
{
    public class FilmeDtoValidator : AbstractValidator<FilmeDto>
    {
        public FilmeDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O título do filme é obrigatório")
                .MaximumLength(210)
                .WithMessage("O título não pode ter mais que 210 caracteres");

            RuleFor(x => x.AnoLancamento)
                .NotEmpty()
                .WithMessage("O ano de lançamento é obrigatório")
                .LessThan(1800)
                .WithMessage("O ano do filme não pode antes de 1800");

            RuleFor(x => x.Genero)
                .NotEmpty()
                .WithMessage("O gênero é obrigatório")
                .MaximumLength(50)
                .WithMessage("O gênero não pode ter mais que 50 caracteres");
        }
    }
}

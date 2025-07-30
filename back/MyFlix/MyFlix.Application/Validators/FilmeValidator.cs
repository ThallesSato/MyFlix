using FluentValidation;
using MyFlix.Domain.Models;

namespace MyFlix.Application.Validators
{
    public class FilmeValidator : AbstractValidator<Filme>
    {
        public FilmeValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O título do filme é obrigatório")
                .MaximumLength(210)
                .WithMessage("O título não pode ter mais que 210 caracteres");

            RuleFor(x => x.AnoLancamento)
                .NotEmpty()
                .WithMessage("O ano de lançamento é obrigatório")
                .GreaterThan(1800)
                .WithMessage("O ano do filme não pode antes de 1800");

            RuleFor(x => x.Genero)
                .NotEmpty()
                .WithMessage("O gênero é obrigatório")
                .MaximumLength(50)
                .WithMessage("O gênero não pode ter mais que 50 caracteres");
            
            RuleFor(x => x.Nota)
                .Null()
                .When(x => !x.Status)
                .WithMessage("A nota deve ser nula quando o status é falso")
                .NotNull()
                .When(x => x.Status)
                .WithMessage("A nota é obrigatória quando o status é verdadeiro")
                .Must((filme, nota) => !filme.Status || (nota >= 1 && nota <= 5))
                .WithMessage("A nota deve estar entre 1 e 5 quando o status é verdadeiro");

        }
    }
}

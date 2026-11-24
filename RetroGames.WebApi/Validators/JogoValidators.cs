using FluentValidation;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Validators;

// Só formato (RN06: preço > 0, estoque >= 0, ano 1970 a 2100).
// Plataforma, condição e gêneros existirem (RN05) é regra de negócio: fica no JogoService.
public class InserirJogoRequestValidator : AbstractValidator<InserirJogoRequestApiModel>
{
    public InserirJogoRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Informe o nome.")
            .MaximumLength(120).WithMessage("O nome deve ter no máximo 120 caracteres.");
        RuleFor(x => x.Descricao)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");
        RuleFor(x => x.AnoLancamento)
            .InclusiveBetween(1970, 2100).WithMessage("Ano de lançamento inválido.");
        RuleFor(x => x.Preco)
            .GreaterThan(0m).WithMessage("O preço deve ser maior que zero.")
            .LessThanOrEqualTo(99999999.99m).WithMessage("Preço inválido.");
        RuleFor(x => x.Estoque)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo.");
        RuleFor(x => x.IdPlataforma)
            .GreaterThan(0).WithMessage("Informe a plataforma.");
        RuleFor(x => x.IdCondicaoJogo)
            .GreaterThan(0).WithMessage("Informe a condição.");
        RuleFor(x => x.IdsGeneros)
            .NotEmpty().WithMessage("Informe ao menos um gênero.");
        RuleForEach(x => x.IdsGeneros)
            .GreaterThan(0).WithMessage("Gênero inválido.");
    }
}

public class AlterarJogoRequestValidator : AbstractValidator<AlterarJogoRequestApiModel>
{
    public AlterarJogoRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Informe o nome.")
            .MaximumLength(120).WithMessage("O nome deve ter no máximo 120 caracteres.");
        RuleFor(x => x.Descricao)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");
        RuleFor(x => x.AnoLancamento)
            .InclusiveBetween(1970, 2100).WithMessage("Ano de lançamento inválido.");
        RuleFor(x => x.Preco)
            .GreaterThan(0m).WithMessage("O preço deve ser maior que zero.")
            .LessThanOrEqualTo(99999999.99m).WithMessage("Preço inválido.");
        RuleFor(x => x.Estoque)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo.");
        RuleFor(x => x.IdPlataforma)
            .GreaterThan(0).WithMessage("Informe a plataforma.");
        RuleFor(x => x.IdCondicaoJogo)
            .GreaterThan(0).WithMessage("Informe a condição.");
        RuleFor(x => x.IdsGeneros)
            .NotEmpty().WithMessage("Informe ao menos um gênero.");
        RuleForEach(x => x.IdsGeneros)
            .GreaterThan(0).WithMessage("Gênero inválido.");
    }
}

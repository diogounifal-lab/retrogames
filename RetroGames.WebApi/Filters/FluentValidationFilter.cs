using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RetroGames.WebApi.Filters;

/// <summary>
/// Roda depois do model binding e antes da action: procura um IValidator para cada argumento.
/// Com erro, responde 400 (código 3) e a action não roda.
/// </summary>
public class FluentValidationFilter(IServiceProvider servicos) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext contexto, ActionExecutionDelegate proximo)
    {
        var erros = new List<(string Campo, string Mensagem)>();

        foreach (var argumento in contexto.ActionArguments.Values)
        {
            if (argumento is null) continue;

            var tipoValidator = typeof(IValidator<>).MakeGenericType(argumento.GetType());
            if (servicos.GetService(tipoValidator) is not IValidator validator) continue;

            var resultado = await validator.ValidateAsync(
                new ValidationContext<object>(argumento), contexto.HttpContext.RequestAborted);

            erros.AddRange(resultado.Errors.Select(e => (e.PropertyName, e.ErrorMessage)));
        }

        if (erros.Count > 0)
        {
            contexto.Result = RespostaValidacao.Criar(erros);
            return;
        }

        await proximo();
    }
}

using Microsoft.AspNetCore.Diagnostics;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Middlewares;

/// <summary>Único ponto que transforma exceção em resposta HTTP (sem try/catch nos controllers).</summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int status;
        RetroGamesResponseApiModel resposta;

        if (exception is BusinessRuleException negocio)
        {
            status = (int)negocio.Erro.Status;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = negocio.Erro.Code,
                ErrorMessage = negocio.Message,
            };
        }
        else
        {
            // Os detalhes vão só para o log, nunca para o cliente.
            logger.LogError(exception, "Erro não tratado em {Metodo} {Caminho}",
                httpContext.Request.Method, httpContext.Request.Path);
            status = StatusCodes.Status500InternalServerError;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.ERRO,
                ErrorMessage = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
            };
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(resposta, cancellationToken);
        return true;
    }
}

using Microsoft.AspNetCore.Mvc;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Filters;

/// <summary>Monta a resposta 400 com responseCode 3 e os erros por campo (usada pelo filtro e pelo [ApiController]).</summary>
public static class RespostaValidacao
{
    public static ObjectResult Criar(IEnumerable<(string Campo, string Mensagem)> erros)
    {
        var porCampo = erros
            .GroupBy(e => NomeDoCampo(e.Campo))
            .ToDictionary(g => g.Key, g => g.Select(e => e.Mensagem).Distinct().ToArray());

        var envelope = new RetroGamesResponseApiModel<Dictionary<string, string[]>>(porCampo)
        {
            ResponseCode = ResponseCodes.MODEL_STATE_INVALIDO.Code,
            ErrorMessage = ResponseCodes.MODEL_STATE_INVALIDO.DefaultMessage,
        };
        return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
    }

    /// <summary>"Nome" → "nome"; "$.nome" → "nome"; vazio ou "$" (JSON malformado) → "corpo".</summary>
    private static string NomeDoCampo(string campo)
    {
        campo = campo.StartsWith("$.") ? campo[2..] : campo;
        if (string.IsNullOrWhiteSpace(campo) || campo == "$") return "corpo";
        return char.ToLowerInvariant(campo[0]) + campo[1..];
    }
}

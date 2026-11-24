using System.Text.Json.Serialization;

namespace RetroGames.WebApi.ApiModels;

/// <summary>Envelope sem dados (erros e exclusões).</summary>
public class RetroGamesResponseApiModel
{
    public int ResponseCode { get; set; } = ResponseCodes.SUCESSO;
    public string? ErrorMessage { get; set; }
}

/// <summary>Envelope com dados.</summary>
public class RetroGamesResponseApiModel<T> : RetroGamesResponseApiModel
{
    public RetroGamesResponseApiModel() { }

    public RetroGamesResponseApiModel(T data) => Data = data;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }
}

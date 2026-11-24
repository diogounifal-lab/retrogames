using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

public static class PlataformaMapper
{
    public static ListarPlataformaResponseApiModel ParaResponse(this Plataforma plataforma) => new()
    {
        Id = plataforma.Id,
        Nome = plataforma.Nome,
        Fabricante = plataforma.Fabricante,
        AnoLancamento = plataforma.AnoLancamento,
    };

    public static Plataforma ParaModel(this InserirPlataformaRequestApiModel request) => new()
    {
        Nome = request.Nome,
        Fabricante = request.Fabricante,
        AnoLancamento = request.AnoLancamento,
    };

    // Na alteração, o id vem da rota.
    public static Plataforma ParaModel(this AlterarPlataformaRequestApiModel request, int id) => new()
    {
        Id = id,
        Nome = request.Nome,
        Fabricante = request.Fabricante,
        AnoLancamento = request.AnoLancamento,
    };
}

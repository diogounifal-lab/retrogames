namespace RetroGames.WebApi.ApiModels;

/// <summary>Entrada do POST /api/Plataforma. Sem Id: o banco gera.</summary>
public class InserirPlataformaRequestApiModel
{
    public string Nome { get; set; } = string.Empty;
    public string Fabricante { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
}

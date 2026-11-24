using RetroGames.Model.Model;

namespace RetroGames.Services;

/// <summary>Jogo com os dados de outras tabelas, montado no Service com LINQ.</summary>
public class JogoDetalheServiceModel
{
    public Jogo Jogo { get; set; } = new();
    public string NomePlataforma { get; set; } = string.Empty;
    public string DescricaoCondicao { get; set; } = string.Empty;
    public List<Genero> Generos { get; set; } = new();
}

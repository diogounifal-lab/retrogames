namespace RetroGames.Services;

/// <summary>Filtros e paginação da listagem. incluirInativos entra na aula 6 (autenticação).</summary>
public class JogoFiltroServiceModel
{
    public string? Busca { get; set; }
    public int? IdPlataforma { get; set; }
    public int? IdGenero { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 12;
}

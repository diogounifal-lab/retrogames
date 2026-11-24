namespace RetroGames.Services;

public class PaginaServiceModel<T>
{
    public IReadOnlyList<T> Itens { get; set; } = new List<T>();
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
}

using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IJogoRepository
{
    /// <summary>
    /// Página de jogos e total encontrado, com o mesmo filtro. idsJogos nulo = sem filtro por ids;
    /// lista vazia = nenhum jogo (o filtro de gênero não achou nada).
    /// </summary>
    Task<(IReadOnlyList<Jogo> Itens, int Total)> ListarAsync(
        string? busca, int? idPlataforma, IReadOnlyCollection<int>? idsJogos,
        bool incluirInativos, int pagina, int tamanhoPagina);

    Task<Jogo?> BuscarPorIdAsync(int id);
    Task<int> InserirAsync(Jogo jogo);
    Task AlterarAsync(Jogo jogo);
    Task DesativarAsync(int id);
}

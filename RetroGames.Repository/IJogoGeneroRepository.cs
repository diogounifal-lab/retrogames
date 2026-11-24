using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IJogoGeneroRepository
{
    Task<IReadOnlyList<JogoGenero>> ListarTodosAsync();
    Task<IReadOnlyList<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogos);
    Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros);
    Task RemoverPorJogoAsync(int idJogo);
}

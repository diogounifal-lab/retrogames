using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface ICondicaoJogoRepository
{
    Task<IReadOnlyList<CondicaoJogo>> ListarAsync();
}

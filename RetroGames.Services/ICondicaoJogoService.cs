using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface ICondicaoJogoService
{
    Task<IReadOnlyList<CondicaoJogo>> ListarAsync();
}

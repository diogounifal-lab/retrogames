using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class CondicaoJogoService(IRepositorySet repositorio) : ICondicaoJogoService
{
    public async Task<IReadOnlyList<CondicaoJogo>> ListarAsync() => await repositorio.CondicaoJogo.ListarAsync();
}

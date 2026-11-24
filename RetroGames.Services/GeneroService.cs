using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class GeneroService(IRepositorySet repositorio) : IGeneroService
{
    public async Task<IReadOnlyList<Genero>> ListarAsync() => await repositorio.Genero.ListarAsync();
}

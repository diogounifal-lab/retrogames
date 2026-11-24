using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface IGeneroService
{
    Task<IReadOnlyList<Genero>> ListarAsync();
}

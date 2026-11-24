using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IGeneroRepository
{
    Task<IReadOnlyList<Genero>> ListarAsync();
}

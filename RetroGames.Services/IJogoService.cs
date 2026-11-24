using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface IJogoService
{
    Task<PaginaServiceModel<JogoDetalheServiceModel>> ListarAsync(JogoFiltroServiceModel filtro);
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);
    Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros);
    Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros);
    Task DesativarAsync(int id);
}

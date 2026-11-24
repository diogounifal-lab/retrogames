using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoGeneroRepository(RepositoryConfig config) : RepositoryBase(config), IJogoGeneroRepository
{
    public async Task<IReadOnlyList<JogoGenero>> ListarTodosAsync()
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "SELECT co_jogo, co_genero FROM tb_jogo_genero");
        return await LerAsync(comando);
    }

    public async Task<IReadOnlyList<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogos)
    {
        if (idsJogos.Count == 0)
            return new List<JogoGenero>();

        var ids = idsJogos.ToList();
        var nomes = ids.Select((_, i) => $"@idJogo{i}");
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            $"SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo IN ({string.Join(", ", nomes)})");
        for (var i = 0; i < ids.Count; i++)
            AdicionarParametro(comando, $"@idJogo{i}", ids[i]);
        return await LerAsync(comando);
    }

    public async Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros)
    {
        await using var conexao = await AbrirConexaoAsync();
        foreach (var idGenero in idsGeneros)
        {
            await using var comando = CriarComando(conexao,
                "INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES (@idJogo, @idGenero)");
            AdicionarParametro(comando, "@idJogo", idJogo);
            AdicionarParametro(comando, "@idGenero", idGenero);
            await ExecutarGravacaoAsync(comando);
        }
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo");
        AdicionarParametro(comando, "@idJogo", idJogo);
        await comando.ExecuteNonQueryAsync();
    }

    private static async Task<IReadOnlyList<JogoGenero>> LerAsync(MySqlCommand comando)
    {
        await using var leitor = await comando.ExecuteReaderAsync();
        var lista = new List<JogoGenero>();
        while (await leitor.ReadAsync())
            lista.Add(new JogoGenero { IdJogo = leitor.GetInt32("co_jogo"), IdGenero = leitor.GetInt32("co_genero") });
        return lista;
    }
}

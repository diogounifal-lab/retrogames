using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class PlataformaRepository(RepositoryConfig config) : RepositoryBase(config), IPlataformaRepository
{
    private const string Colunas = "co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento";

    public async Task<IReadOnlyList<Plataforma>> ListarAsync()
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, $"SELECT {Colunas} FROM tb_plataforma ORDER BY no_plataforma");
        await using var leitor = await comando.ExecuteReaderAsync();
        var lista = new List<Plataforma>();
        while (await leitor.ReadAsync())
            lista.Add(Mapear(leitor));
        return lista;
    }

    public async Task<Plataforma?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, $"SELECT {Colunas} FROM tb_plataforma WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();
        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT COUNT(*) FROM tb_plataforma WHERE no_plataforma = @nome " +
            "AND (@idIgnorar IS NULL OR co_seq_plataforma <> @idIgnorar)");
        AdicionarParametro(comando, "@nome", nome);
        AdicionarParametro(comando, "@idIgnorar", idIgnorar);
        return Convert.ToInt32(await comando.ExecuteScalarAsync()) > 0;
    }

    public async Task<bool> PossuiJogosAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "SELECT COUNT(*) FROM tb_jogo WHERE co_plataforma = @id");
        AdicionarParametro(comando, "@id", id);
        return Convert.ToInt32(await comando.ExecuteScalarAsync()) > 0;
    }

    public async Task<int> InserirAsync(Plataforma plataforma)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) " +
            "VALUES (@nome, @fabricante, @ano)");
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@ano", plataforma.AnoLancamento);
        await ExecutarGravacaoAsync(comando);
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Plataforma plataforma)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_plataforma SET no_plataforma = @nome, no_fabricante = @fabricante, " +
            "nu_ano_lancamento = @ano WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@id", plataforma.Id);
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@ano", plataforma.AnoLancamento);
        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "DELETE FROM tb_plataforma WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@id", id);
        await comando.ExecuteNonQueryAsync();
    }

    private static Plataforma Mapear(MySqlDataReader leitor) => new()
    {
        Id = leitor.GetInt32("co_seq_plataforma"),
        Nome = leitor.GetString("no_plataforma"),
        Fabricante = leitor.GetString("no_fabricante"),
        AnoLancamento = leitor.GetInt32("nu_ano_lancamento"),
    };
}

using System.Text;
using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoRepository(RepositoryConfig config) : RepositoryBase(config), IJogoRepository
{
    private const string Colunas =
        "co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, " +
        "dt_cadastro, co_plataforma, co_condicao_jogo";

    public async Task<(IReadOnlyList<Jogo> Itens, int Total)> ListarAsync(
        string? busca, int? idPlataforma, IReadOnlyCollection<int>? idsJogos,
        bool incluirInativos, int pagina, int tamanhoPagina)
    {
        if (idsJogos is { Count: 0 })
            return (new List<Jogo>(), 0);

        // O texto do WHERE só tem trechos fixos; todo valor entra por parâmetro.
        var where = new StringBuilder(" WHERE 1 = 1");
        if (!incluirInativos) where.Append(" AND st_ativo = TRUE");
        var temBusca = !string.IsNullOrWhiteSpace(busca);
        if (temBusca) where.Append(" AND no_jogo LIKE @busca");
        if (idPlataforma.HasValue) where.Append(" AND co_plataforma = @idPlataforma");
        var listaIds = idsJogos?.ToList();
        if (listaIds is not null)
            where.Append(" AND co_seq_jogo IN (")
                 .Append(string.Join(", ", listaIds.Select((_, i) => $"@idJogo{i}")))
                 .Append(')');

        void AdicionarFiltros(MySqlCommand comando)
        {
            if (temBusca)
            {
                var texto = busca!.Trim().Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
                AdicionarParametro(comando, "@busca", $"%{texto}%");
            }
            if (idPlataforma.HasValue) AdicionarParametro(comando, "@idPlataforma", idPlataforma.Value);
            if (listaIds is not null)
                for (var i = 0; i < listaIds.Count; i++)
                    AdicionarParametro(comando, $"@idJogo{i}", listaIds[i]);
        }

        await using var conexao = await AbrirConexaoAsync();

        int total;
        await using (var contagem = CriarComando(conexao, "SELECT COUNT(*) FROM tb_jogo" + where))
        {
            AdicionarFiltros(contagem);
            total = Convert.ToInt32(await contagem.ExecuteScalarAsync());
        }

        var itens = new List<Jogo>();
        await using (var consulta = CriarComando(conexao,
            $"SELECT {Colunas} FROM tb_jogo{where} ORDER BY no_jogo, co_seq_jogo LIMIT @limite OFFSET @deslocamento"))
        {
            AdicionarFiltros(consulta);
            AdicionarParametro(consulta, "@limite", tamanhoPagina);
            AdicionarParametro(consulta, "@deslocamento", (pagina - 1) * tamanhoPagina);
            await using var leitor = await consulta.ExecuteReaderAsync();
            while (await leitor.ReadAsync())
                itens.Add(Mapear(leitor));
        }

        return (itens, total);
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, $"SELECT {Colunas} FROM tb_jogo WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();
        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, " +
            "dt_cadastro, co_plataforma, co_condicao_jogo) " +
            "VALUES (@nome, @descricao, @ano, @preco, @estoque, @ativo, @dataCadastro, @idPlataforma, @idCondicao)");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@ano", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@dataCadastro", jogo.DataCadastro);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicao", jogo.IdCondicaoJogo);
        await ExecutarGravacaoAsync(comando);
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET no_jogo = @nome, ds_jogo = @descricao, nu_ano_lancamento = @ano, " +
            "vl_preco = @preco, qt_estoque = @estoque, st_ativo = @ativo, " +
            "co_plataforma = @idPlataforma, co_condicao_jogo = @idCondicao WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", jogo.Id);
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@ano", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicao", jogo.IdCondicaoJogo);
        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);
        await comando.ExecuteNonQueryAsync();
    }

    private static Jogo Mapear(MySqlDataReader leitor) => new()
    {
        Id = leitor.GetInt32("co_seq_jogo"),
        Nome = leitor.GetString("no_jogo"),
        Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_jogo")) ? null : leitor.GetString("ds_jogo"),
        AnoLancamento = leitor.GetInt32("nu_ano_lancamento"),
        Preco = leitor.GetDecimal("vl_preco"),
        Estoque = leitor.GetInt32("qt_estoque"),
        Ativo = leitor.GetBoolean("st_ativo"),
        DataCadastro = leitor.GetDateTime("dt_cadastro"),
        IdPlataforma = leitor.GetInt32("co_plataforma"),
        IdCondicaoJogo = leitor.GetInt32("co_condicao_jogo"),
    };
}

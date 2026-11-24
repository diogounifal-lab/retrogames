using MySqlConnector;

namespace RetroGames.Repository;

public abstract class RepositoryBase(RepositoryConfig config)
{
    /// <summary>Abre uma conexão. Quem chama a fecha com await using.</summary>
    protected async Task<MySqlConnection> AbrirConexaoAsync()
    {
        var conexao = new MySqlConnection(config.ConnectionString);
        try
        {
            await conexao.OpenAsync();
            return conexao;
        }
        catch
        {
            await conexao.DisposeAsync();
            throw;
        }
    }

    protected static MySqlCommand CriarComando(MySqlConnection conexao, string sql) => new(sql, conexao);

    protected static void AdicionarParametro(MySqlCommand comando, string nome, object? valor) =>
        comando.Parameters.AddWithValue(nome, valor ?? DBNull.Value);

    /// <summary>Executa INSERT/UPDATE. Converte o erro 1062 (UNIQUE) em RegistroDuplicadoException.</summary>
    protected static async Task<int> ExecutarGravacaoAsync(MySqlCommand comando)
    {
        try
        {
            return await comando.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            throw new RegistroDuplicadoException(ex);
        }
    }
}

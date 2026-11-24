using System.Transactions;
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class JogoService(IRepositorySet repositorio) : IJogoService
{
    private const int TamanhoPaginaPadrao = 12;
    private const int TamanhoPaginaMaximo = 100;

    public async Task<PaginaServiceModel<JogoDetalheServiceModel>> ListarAsync(JogoFiltroServiceModel filtro)
    {
        var pagina = Math.Max(1, filtro.Pagina);
        var tamanho = filtro.TamanhoPagina < 1 ? TamanhoPaginaPadrao : Math.Min(filtro.TamanhoPagina, TamanhoPaginaMaximo);

        // Filtro por gênero depende de outra tabela: resolvido aqui com LINQ,
        // e o resultado segue para o SQL como lista de ids (paginação e contagem ficam no banco).
        IReadOnlyCollection<int>? idsJogos = null;
        if (filtro.IdGenero.HasValue)
        {
            var vinculos = await repositorio.JogoGenero.ListarTodosAsync();
            idsJogos = vinculos
                .Where(v => v.IdGenero == filtro.IdGenero.Value)
                .Select(v => v.IdJogo)
                .Distinct()
                .ToList();
        }

        // Inativos só para o administrador: fica para a aula 6 (por enquanto, sempre false).
        var (jogos, total) = await repositorio.Jogo.ListarAsync(
            filtro.Busca, filtro.IdPlataforma, idsJogos, incluirInativos: false, pagina, tamanho);

        return new PaginaServiceModel<JogoDetalheServiceModel>
        {
            Itens = await MontarDetalhesAsync(jogos),
            Pagina = pagina,
            TamanhoPagina = tamanho,
            TotalItens = total,
        };
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await repositorio.Jogo.BuscarPorIdAsync(id)
                   ?? throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        return (await MontarDetalhesAsync([jogo]))[0];
    }

    public async Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros)
    {
        jogo.Nome = jogo.Nome.Trim();
        jogo.Ativo = true;
        jogo.DataCadastro = DateTime.UtcNow;
        var generos = idsGeneros.Distinct().ToList();

        await ValidarReferenciasAsync(jogo.IdPlataforma, jogo.IdCondicaoJogo, generos);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await repositorio.Jogo.InserirAsync(jogo);
            await repositorio.JogoGenero.InserirAsync(jogo.Id, generos);
            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros)
    {
        if (await repositorio.Jogo.BuscarPorIdAsync(jogo.Id) is null)
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        jogo.Nome = jogo.Nome.Trim();
        var generos = idsGeneros.Distinct().ToList();

        await ValidarReferenciasAsync(jogo.IdPlataforma, jogo.IdCondicaoJogo, generos);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            await repositorio.Jogo.AlterarAsync(jogo);
            await repositorio.JogoGenero.RemoverPorJogoAsync(jogo.Id);
            await repositorio.JogoGenero.InserirAsync(jogo.Id, generos);
            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task DesativarAsync(int id)
    {
        if (await repositorio.Jogo.BuscarPorIdAsync(id) is null)
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        // Exclusão lógica (RN03): o jogo nunca é apagado.
        await repositorio.Jogo.DesativarAsync(id);
    }

    /// <summary>RN05: plataforma, condição e gêneros precisam existir.</summary>
    private async Task ValidarReferenciasAsync(int idPlataforma, int idCondicao, IReadOnlyCollection<int> idsGeneros)
    {
        if (await repositorio.Plataforma.BuscarPorIdAsync(idPlataforma) is null)
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);

        var condicoes = await repositorio.CondicaoJogo.ListarAsync();
        if (condicoes.All(c => c.Id != idCondicao))
            throw new BusinessRuleException(BusinessErrors.CONDICAO_NAO_ENCONTRADA);

        var existentes = (await repositorio.Genero.ListarAsync()).Select(g => g.Id).ToHashSet();
        if (idsGeneros.Any(id => !existentes.Contains(id)))
            throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
    }

    /// <summary>Monta o detalhe com LINQ, sempre com 4 consultas, qualquer que seja o tamanho da lista.</summary>
    private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(IReadOnlyList<Jogo> jogos)
    {
        if (jogos.Count == 0)
            return [];

        var plataformas = (await repositorio.Plataforma.ListarAsync()).ToDictionary(p => p.Id);
        var condicoes = (await repositorio.CondicaoJogo.ListarAsync()).ToDictionary(c => c.Id);
        var generos = (await repositorio.Genero.ListarAsync()).ToDictionary(g => g.Id);
        var vinculos = (await repositorio.JogoGenero.ListarPorJogosAsync(jogos.Select(j => j.Id).ToList()))
            .ToLookup(v => v.IdJogo);

        return jogos.Select(j => new JogoDetalheServiceModel
        {
            Jogo = j,
            NomePlataforma = plataformas.TryGetValue(j.IdPlataforma, out var plataforma) ? plataforma.Nome : string.Empty,
            DescricaoCondicao = condicoes.TryGetValue(j.IdCondicaoJogo, out var condicao) ? condicao.Descricao : string.Empty,
            Generos = vinculos[j.Id]
                .Where(v => generos.ContainsKey(v.IdGenero))
                .Select(v => generos[v.IdGenero])
                .OrderBy(g => g.Nome)
                .ToList(),
        }).ToList();
    }
}

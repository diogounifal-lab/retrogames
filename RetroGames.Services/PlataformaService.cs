using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class PlataformaService(IRepositorySet repositorio) : IPlataformaService
{
    public async Task<IReadOnlyList<Plataforma>> ListarAsync() =>
        await repositorio.Plataforma.ListarAsync();

    public async Task<Plataforma> BuscarPorIdAsync(int id) =>
        await repositorio.Plataforma.BuscarPorIdAsync(id)
        ?? throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);

    public async Task<Plataforma> InserirAsync(Plataforma plataforma)
    {
        plataforma.Nome = plataforma.Nome.Trim();

        if (await repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome))
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);

        try
        {
            plataforma.Id = await repositorio.Plataforma.InserirAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            // Duas gravações ao mesmo tempo: o UNIQUE do banco recusou a segunda.
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }
        return plataforma;
    }

    public async Task<Plataforma> AlterarAsync(Plataforma plataforma)
    {
        if (await repositorio.Plataforma.BuscarPorIdAsync(plataforma.Id) is null)
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);

        plataforma.Nome = plataforma.Nome.Trim();

        // Ignora a própria plataforma: manter o próprio nome não é duplicidade.
        if (await repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome, plataforma.Id))
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);

        try
        {
            await repositorio.Plataforma.AlterarAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }
        return plataforma;
    }

    public async Task ExcluirAsync(int id)
    {
        if (await repositorio.Plataforma.BuscarPorIdAsync(id) is null)
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);

        if (await repositorio.Plataforma.PossuiJogosAsync(id))
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_POSSUI_JOGOS);

        await repositorio.Plataforma.ExcluirAsync(id);
    }
}

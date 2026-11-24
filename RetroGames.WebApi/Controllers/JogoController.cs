using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JogoController(IJogoService jogoService) : ControllerBase
{
    [HttpGet] // GET /api/Jogo?busca=zelda&idPlataforma=2&pagina=1
    public async Task<ActionResult<RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>>> Listar(
        [FromQuery] ListarJogoRequestApiModel request)
    {
        var pagina = await jogoService.ListarAsync(request.ParaFiltro());
        return Ok(new RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>(
            pagina.ParaPaginaResponse()));
    }

    [HttpGet("{id:int}")] // GET /api/Jogo/7
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> ListarPorId(int id)
    {
        var detalhe = await jogoService.BuscarPorIdAsync(id);
        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(detalhe.ParaDetalheResponse()));
    }

    [HttpPost] // POST /api/Jogo
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> Inserir(
        InserirJogoRequestApiModel request)
    {
        var criado = await jogoService.InserirAsync(request.ParaModel(), request.IdsGeneros);
        var envelope = new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(criado.ParaDetalheResponse());
        return CreatedAtAction(nameof(ListarPorId), new { id = criado.Jogo.Id }, envelope);
    }

    [HttpPut("{id:int}")] // PUT /api/Jogo/7
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> Alterar(
        int id, AlterarJogoRequestApiModel request)
    {
        var alterado = await jogoService.AlterarAsync(request.ParaModel(id), request.IdsGeneros);
        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(alterado.ParaDetalheResponse()));
    }

    [HttpDelete("{id:int}")] // DELETE /api/Jogo/7 (exclusão lógica: só desativa, RN03)
    public async Task<ActionResult<RetroGamesResponseApiModel>> Excluir(int id)
    {
        await jogoService.DesativarAsync(id);
        return Ok(new RetroGamesResponseApiModel());
    }
}

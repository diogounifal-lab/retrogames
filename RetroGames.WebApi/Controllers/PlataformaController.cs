using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlataformaController(IPlataformaService plataformaService) : ControllerBase
{
    [HttpGet] // GET /api/Plataforma
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>>> Listar()
    {
        var plataformas = await plataformaService.ListarAsync();
        return Ok(new RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>(
            plataformas.Select(p => p.ParaResponse()).ToList()));
    }

    [HttpGet("{id:int}")] // GET /api/Plataforma/3
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> ListarPorId(int id)
    {
        var plataforma = await plataformaService.BuscarPorIdAsync(id);
        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(plataforma.ParaResponse()));
    }

    [HttpPost] // POST /api/Plataforma
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> Inserir(
        InserirPlataformaRequestApiModel request)
    {
        var criada = await plataformaService.InserirAsync(request.ParaModel());
        var envelope = new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(criada.ParaResponse());
        return CreatedAtAction(nameof(ListarPorId), new { id = criada.Id }, envelope);
    }

    [HttpPut("{id:int}")] // PUT /api/Plataforma/3
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> Alterar(
        int id, AlterarPlataformaRequestApiModel request)
    {
        var alterada = await plataformaService.AlterarAsync(request.ParaModel(id));
        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(alterada.ParaResponse()));
    }

    [HttpDelete("{id:int}")] // DELETE /api/Plataforma/3
    public async Task<ActionResult<RetroGamesResponseApiModel>> Excluir(int id)
    {
        await plataformaService.ExcluirAsync(id);
        return Ok(new RetroGamesResponseApiModel());
    }
}

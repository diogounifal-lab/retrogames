using System.Net;

namespace RetroGames.Services;

/// <summary>Um erro de negócio: código (a partir de 100), mensagem padrão e status HTTP.</summary>
public sealed record BusinessError(int Code, string DefaultMessage, HttpStatusCode Status);

/// <summary>Catálogo dos erros de negócio. Próximo código livre: 106.</summary>
public static class BusinessErrors
{
    public static readonly BusinessError PLATAFORMA_NAO_ENCONTRADA =
        new(100, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    public static readonly BusinessError PLATAFORMA_NOME_DUPLICADO =
        new(101, "Já existe uma plataforma com este nome.", HttpStatusCode.Conflict);

    public static readonly BusinessError PLATAFORMA_POSSUI_JOGOS =
        new(102, "Não é possível excluir: existem jogos desta plataforma.", HttpStatusCode.Conflict);

    public static readonly BusinessError JOGO_NAO_ENCONTRADO =
        new(103, "Jogo não encontrado.", HttpStatusCode.NotFound);

    public static readonly BusinessError CONDICAO_NAO_ENCONTRADA =
        new(104, "Condição de jogo não encontrada.", HttpStatusCode.NotFound);

    public static readonly BusinessError GENERO_NAO_ENCONTRADO =
        new(105, "Gênero não encontrado.", HttpStatusCode.NotFound);
}

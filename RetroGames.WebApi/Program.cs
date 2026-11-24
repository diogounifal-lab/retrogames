using FluentValidation;
using RetroGames.WebApi.Configuration;
using RetroGames.WebApi.Filters;
using RetroGames.WebApi.Middlewares;

// O .env precisa ser carregado ANTES de criar o builder, para a configuração enxergá-lo.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException(
        "Connection string não encontrada. Crie o arquivo .env na raiz do repositório com a linha " +
        "ConnectionStrings__Default=\"...\" (veja o modelo .env.example).");

builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();

// Aula 5: validators de formato + filtro que os executa antes da action.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services
    .AddControllers(opcoes =>
    {
        opcoes.Filters.Add<FluentValidationFilter>();
        // Campo ausente vira valor padrão e é o validator que reclama, com a mensagem em português.
        opcoes.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .ConfigureApiBehaviorOptions(opcoes =>
    {
        // JSON malformado ou tipo errado: 400, código 3, no formato do envelope.
        opcoes.InvalidModelStateResponseFactory = contexto =>
        {
            var erros = contexto.ModelState
                .Where(e => e.Value is { Errors.Count: > 0 })
                .SelectMany(e => e.Value!.Errors.Select(erro => (
                    Campo: e.Key,
                    Mensagem: string.IsNullOrWhiteSpace(e.Key) || e.Key.StartsWith('$') || string.IsNullOrWhiteSpace(erro.ErrorMessage)
                        ? "Valor inválido ou JSON malformado."
                        : erro.ErrorMessage)));
            return RespostaValidacao.Criar(erros);
        };
    });

builder.Services.AddOpenApi();

// Aula 4: tratamento global de erros.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(); // primeira etapa do pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opcoes =>
    {
        opcoes.SwaggerEndpoint("/openapi/v1.json", "RetroGames v1");
        opcoes.RoutePrefix = "swagger";
    });
}

// Aula 5: as rotas de gravação ficam abertas até a aula 6 (login de administrador).
app.MapControllers();

app.Run();

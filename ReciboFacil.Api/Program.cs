using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Azure.AI.OpenAI;
using ReciboFacil.Aplicacao;
using ReciboFacil.Repositorio;
using ReciboFacil.Servicos.Implementacoes;
using ReciboFacil.Servicos.Interfaces;
using Azure.Core.Pipeline;

var builder = WebApplication.CreateBuilder(args);

// Configurações do banco de dados (mantido igual)
builder.Services.AddDbContext<ReciboFacilContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios (mantido igual)
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IReciboRepositorio, ReciboRepositorio>();
builder.Services.AddScoped<IItemReciboRepositorio, ItemReciboRepositorio>();

// Aplicação (mantido igual)
builder.Services.AddScoped<IClienteAplicacao, ClienteAplicacao>();
builder.Services.AddScoped<IProdutoAplicacao, ProdutoAplicacao>();
builder.Services.AddScoped<IReciboAplicacao, ReciboAplicacao>();
builder.Services.AddScoped<IItemReciboAplicacao, ItemReciboAplicacao>();

// Serviços (mantido igual)
builder.Services.AddMemoryCache();

// ************ ÁREA MODIFICADA (OpenAI) ************
builder.Services.AddSingleton<OpenAIClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();

    // 1. Prioridade: variável de ambiente > appsettings.json
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                 ?? config["OpenAI:ApiKey"];

    // 2. Validação obrigatória
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new InvalidOperationException(
            "Chave da OpenAI não configurada. Defina em:\n" +
            "1. launchSettings.json (Development) ou\n" +
            "2. Variáveis de ambiente do sistema (Production)");
    }

    // 3. Configuração do cliente
    return new OpenAIClient(apiKey, new OpenAIClientOptions
    {
        RetryPolicy = new RetryPolicy(maxRetries: 3),
        Diagnostics = { ApplicationId = "ReciboFacil" }
    });
});
// ************ FIM DAS ALTERAÇÕES ************

builder.Services.AddScoped<IAIService, OpenAIService>();

// Configurações adicionais (mantido igual)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ReciboFacil API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Endpoint de debug apenas para desenvolvimento (opcional)
    app.MapGet("/debug/openai-config", () =>
    {
        var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        return Results.Json(new
        {
            KeyConfigured = !string.IsNullOrEmpty(key),
            Source = key != null ? "Environment" : "appsettings.json"
        });
    }).ExcludeFromDescription();
}

app.MapGet("/debug-env", () =>
    Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "Variável não encontrada!");

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
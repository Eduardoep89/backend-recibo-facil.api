using System.Net.Http.Headers;
using System.Text.Json.Serialization; // Adicionado para ReferenceHandler
using Microsoft.EntityFrameworkCore;
using Azure.AI.OpenAI;
using ReciboFacil.Aplicacao;
using ReciboFacil.Repositorio;
using ReciboFacil.Servicos.Implementacoes;
using ReciboFacil.Servicos.Interfaces;
using Azure.Core.Pipeline;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies; // Adicionado para JsonSerializerOptions
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio.Interfaces;
using ReciboFacil.Aplicacao.Interfaces;
using ReciboFacil.Repositorio.Repositorios;
using ReciboFacil.Aplicacao.Aplicacoes;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Configurações do banco de dados
builder.Services.AddDbContext<ReciboFacilContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios e Aplicação
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IReciboRepositorio, ReciboRepositorio>();
builder.Services.AddScoped<IItemReciboRepositorio, ItemReciboRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Aplicação
builder.Services.AddScoped<IClienteAplicacao, ClienteAplicacao>();
builder.Services.AddScoped<IProdutoAplicacao, ProdutoAplicacao>();
builder.Services.AddScoped<IReciboAplicacao, ReciboAplicacao>();
builder.Services.AddScoped<IItemReciboAplicacao, ItemReciboAplicacao>();
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();

// Serviços
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache(); // Necessário para a sessão
builder.Services.AddScoped<IAutenticacaoServico, AutenticacaoServico>();
// Configuração da OpenAI (Versão Corrigida)
builder.Services.AddSingleton<OpenAIClient>(provider =>
{
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ??
                builder.Configuration["OpenAI:ApiKey"];

    if (string.IsNullOrEmpty(apiKey) && builder.Environment.IsDevelopment())
    {
        apiKey = "sua-chave-mock-dev";
        Console.WriteLine("AVISO: Usando chave mock para desenvolvimento");
    }

    if (string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException(
            "Chave OpenAI não configurada. Configure:\n" +
            "1. Variável de ambiente: OPENAI_API_KEY\n" +
            "2. appsettings.json: \"OpenAI\": { \"ApiKey\": \"sua-chave\" }");
    }

    return new OpenAIClient(apiKey, new OpenAIClientOptions
    {
        RetryPolicy = new RetryPolicy(maxRetries: 2),
        Transport = new HttpClientTransport(new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(30) // Timeout aumentado
        })
    });
});

builder.Services.AddScoped<IAIService, OpenAIService>();

// Configurações de CORS melhoradas
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configuração de controllers com serialização otimizada
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configuração do HttpClient para chamadas externas
builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ReciboFacil API", Version = "v1" });

    // Configuração adicional para o Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});
// Autenticação por sessão 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always em produção
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/api/Account/Login";
        options.LogoutPath = "/api/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Tempo da sessão
    });

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(2);
});

var app = builder.Build();

// Pipeline de middlewares na ordem correta
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>  //Habilita a interface UI do Swagger com configurações personalizadas:
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReciboFacil API v1"); //Habilita o middleware do Swagger para gerar a especificação OpenAPI.
        //c.RoutePrefix = string.Empty;
    });

    app.MapGet("/debug/config", () => Results.Json(new
    {
        Database = builder.Configuration.GetConnectionString("DefaultConnection") != null,
        OpenAI = !string.IsNullOrEmpty(builder.Configuration["OpenAI:ApiKey"]),
        Environment = app.Environment.EnvironmentName
    }));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseCors("ReactPolicy");
app.UseAuthentication();

app.MapControllers();

// Rota de health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// Manipulador de erros global
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext context) =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    return Results.Problem(
        title: exception?.Message ?? "An error occurred",
        statusCode: StatusCodes.Status500InternalServerError);
});

app.Run();
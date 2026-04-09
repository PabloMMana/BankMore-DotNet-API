using BankMore.Application.Handlers;
using BankMore.Application.Queries;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Repositories; // Certifique-se que este caminho existe
using BankMore.Infrastructure.Services;
using MediatR;
using Microsoft.OpenApi.Models; // Resolve o erro do OpenApiInfo

var builder = WebApplication.CreateBuilder(args);

// Configuração do MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CadastrarContaHandler).Assembly);
});

// Injeção do Repositório - Verifique se a classe ContaCorrenteRepository existe na Infra
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();

builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => {
      cfg.RegisterServicesFromAssembly(typeof(ObterContasAtivasQuery).Assembly);
});


var app = builder.Build();

// Ativa o Swagger para testar a API visualmente
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
using BankMore.Application.Handlers;
using BankMore.Application.Queries;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Repositories;
using BankMore.Infrastructure.Services;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region 🔧 Serviços

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CadastrarContaHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ObterContasAtivasQuery).Assembly);
});

// Injeção de Dependência
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<TokenService>();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BankMore API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion


#region 🔐 Autenticação JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "BankMore",
            ValidAudience = "BankMoreUsers",

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("SUA_CHAVE_SECRETA_SUPER_FORTE_123456"))
        };
    });

#endregion

var app = builder.Build();


#region 🚀 Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
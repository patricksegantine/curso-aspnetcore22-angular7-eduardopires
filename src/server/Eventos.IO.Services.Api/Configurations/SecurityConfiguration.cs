using Eventos.IO.Infra.CrossCutting.Identity.Data;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Eventos.IO.Infra.CrossCutting.Identity.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Eventos.IO.Services.Api.Configurations
{
    public static class SecurityConfiguration
    {
        public static void AddSecurityConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            if (services == null) throw new ArgumentException(nameof(services));

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de dependências
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            var signinConfigurations = new SigningCredentialsConfigurations();
            services.AddSingleton(signinConfigurations);

            var tokenConfigurations = new JwtTokenConfigurations();
            new ConfigureFromConfigurationOptions<JwtTokenConfigurations>(
                    Configuration.GetSection(nameof(JwtTokenConfigurations)))
                        .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var validatonParameters = options.TokenValidationParameters;
                validatonParameters.IssuerSigningKey = signinConfigurations.Key;
                validatonParameters.ValidateAudience = true;
                validatonParameters.ValidAudience = tokenConfigurations.Audience;
                validatonParameters.ValidateIssuer = true;
                validatonParameters.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                validatonParameters.ValidateIssuerSigningKey = true;

                validatonParameters.RequireExpirationTime = true;

                // Verifica se um token recebido ainda é válido
                validatonParameters.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                validatonParameters.ClockSkew = TimeSpan.Zero;
            });

            // Adiciona as políticas de segurança
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeConsultar", policy => policy.RequireClaim("Eventos", "Consultar"));
                options.AddPolicy("PodeGravar", policy => policy.RequireClaim("Eventos", "Gravar"));
                options.AddPolicy("PodeExcluir", policy => policy.RequireClaim("Eventos", "Excluir"));

                // Ativa o uso do token como forma de autorizar o acesso
                // a recursos deste projeto
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });
        }
    }
}

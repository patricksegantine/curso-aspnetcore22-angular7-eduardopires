using Eventos.IO.Infra.CrossCutting.Identity.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Eventos.IO.Services.Api.Extensions
{
    public static class JwtSecurityExtension
    {
        public static IServiceCollection AddJwtSecurity(
            this IServiceCollection services,
            SigningCredentialsConfigurations signinConfigurations,
            JwtTokenConfigurations tokenConfigurations)
        {
            

            

            return services;


            services.AddMvc(options =>
            {
                // Adiciona a policy no filtro de autenticação
                options.Filters.Add(new AuthorizeFilter(policy));
            
        }
    }
}

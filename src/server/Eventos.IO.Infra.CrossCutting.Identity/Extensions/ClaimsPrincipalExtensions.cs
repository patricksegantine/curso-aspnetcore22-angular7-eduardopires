using System;
using System.Security.Claims;

namespace Eventos.IO.Infra.CrossCutting.Identity.Models
{
    public static class ClaimsPrincipalExtensions
    {
        // ClaimsPrincipal: classe do AspNet (não do Identity) que representa o usuário conectado na aplicação
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }
    }
}

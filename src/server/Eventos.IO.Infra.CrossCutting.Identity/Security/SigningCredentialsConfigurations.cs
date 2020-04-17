using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Eventos.IO.Infra.CrossCutting.Identity.Security
{
    public class SigningCredentialsConfigurations
    {
        private const string SecretKey = "eventosio@meuambienteToken";

        public SymmetricSecurityKey Key { get; private set; } 

        public SigningCredentials SigningCredentials { get; private set; }

        public SigningCredentialsConfigurations()
        {
            Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}

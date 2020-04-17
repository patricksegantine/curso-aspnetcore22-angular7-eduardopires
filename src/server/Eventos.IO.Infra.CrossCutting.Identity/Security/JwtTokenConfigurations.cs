namespace Eventos.IO.Infra.CrossCutting.Identity.Security
{
    // aula19 - 2h06min JwtTokenOptions
    public class JwtTokenConfigurations
    {
        /// <summary>
        /// Emissor
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Pra qual site é válido
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Tempo de duração do token
        /// </summary>
        public int Minutes { get; set; }
    }
}

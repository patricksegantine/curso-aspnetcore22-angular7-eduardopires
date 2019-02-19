using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Domain.Organizadores.Commands;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Eventos.IO.Infra.CrossCutting.Identity.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IBus _bus;

        private readonly JwtTokenConfigurations _jwtTokenConfigurations;
        private readonly SigningCredentialsConfigurations _signingConfigurations;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            [FromServices]JwtTokenConfigurations jwtTokenConfigurations,
            [FromServices]SigningCredentialsConfigurations signingConfigurations,
            IDomainNotificationHandler<DomainNotification> notifications,
            IBus bus,
            IUser user) : base(notifications, bus, user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _bus = bus;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _jwtTokenConfigurations = jwtTokenConfigurations;
            _signingConfigurations = signingConfigurations;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("nova-conta")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model, int version)
        {
            if (!ModelState.IsValid) return Response(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Claims de permissão do usuário
                await _userManager.AddClaimsAsync(user, new[]
                {
                    new Claim("Eventos", "Consultar"),
                    new Claim("Eventos", "Gravar"),
                    new Claim("Eventos", "Excluir")
                });

                // Executando o papel da Camada de Application
                var registroCommand = new RegistrarOrganizadorCommand(Guid.Parse(user.Id), model.Nome, model.CpfCnpj, model.Email);
                _bus.SendCommand(registroCommand);

                if (!OperacaoValida())
                {
                    await _userManager.DeleteAsync(user);
                    return Response(model);
                }

                _logger.LogInformation(1, "Usuário criado com sucesso");

                // Usuário criado com sucesso
                // Retorna o token para não precisar efetuar o login
                var response = await GerarTokenUsuario(new LoginViewModel { Email = model.Email, Password = model.Password });

                return Response(response);
            }

            AdicionarErrosIdentity(result);
            return Response(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotificarErroModelStateInvalido();
                return Response(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"Usuário {model.Email} logado com sucesso");

                // Gera o token do usuário
                var token = await GerarTokenUsuario(model);
                return Response(token);
            }

            NotificarErro(result.ToString(), "Falha ao realizar o login");
            return Response(model);
        }

        #region Helpers

        /// <summary>
        /// Gera o token do usuário
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private async Task<Token> GerarTokenUsuario(LoginViewModel login)
        {
            // Recupera os dados do usuário gravados no banco de dados
            var user = await _userManager.FindByEmailAsync(login.Email);

            // Recupera as claims do usuário gravadas no banco de dados
            var userClaims = await _userManager.GetClaimsAsync(user);

            // e inclui outras claims do token na coleção
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            // Gera o objeto IdentityClaims (necessário)
            var identityClaims = new ClaimsIdentity(new GenericIdentity(user.Email, "Login"), userClaims);

            // Datas criação/expiração do token
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao.AddMinutes(_jwtTokenConfigurations.Minutes);

            // Gera o token
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtTokenConfigurations.Issuer,
                Audience = _jwtTokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identityClaims,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            // Transforma o token em string
            var encodedToken = handler.WriteToken(token);

            // Objeto de resposta
            return new Token
            {
                Authenticated = true,
                AccessToken = encodedToken,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = "Ok"
            };
        }

        /// <summary>
        /// Adiciona os erros capturadores no login ao objeto ao objeto de notificação
        /// </summary>
        /// <param name="result"></param>
        private void AdicionarErrosIdentity(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }
        }

        #endregion
    }
}
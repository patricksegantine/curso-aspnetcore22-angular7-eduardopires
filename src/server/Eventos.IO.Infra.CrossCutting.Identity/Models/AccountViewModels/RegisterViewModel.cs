using System.ComponentModel.DataAnnotations;

namespace Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é requerido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF ou CNPJ é requerido")]
        [StringLength(14, MinimumLength = 11)]
        [Display(Name = "CPF ou CNPJ")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "O e-mail é requerido")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "A {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de senha")]
        [Compare("Password", ErrorMessage = "A senha está diferente.")]
        public string ConfirmPassword { get; set; }
    }
}

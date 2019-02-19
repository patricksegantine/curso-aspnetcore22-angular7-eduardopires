using Eventos.IO.Domain.Core.Models;
using FluentValidation;
using System;

namespace Eventos.IO.Domain.Eventos
{
    public class Endereco : Entity<Endereco>
    {
        protected Endereco() { }

        public Endereco(Guid id, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, Guid eventoId)
        {
            Id = id;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cep = cep;
            Cidade = cidade;
            Estado = estado;
            EventoId = eventoId;
        }

        public string Logradouro { get; private set; }

        public string Numero { get; private set; }

        public string Complemento { get; private set; }

        public string Bairro { get; private set; }

        public string Cep { get; private set; }

        public string Cidade { get; private set; }

        public string Estado { get; private set; }

        public Guid? EventoId { get; private set; }

        // EF Propriedade de Navegação
        public virtual Evento Evento { get; private set; }


        #region Validation

        public override bool IsValid()
        {
            ValidationRules();

            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }

        private void ValidationRules()
        {

            RuleFor(c => c.Cep)
                .NotEmpty().WithMessage("O cep precisa ser fornecido")
                .Length(8).WithMessage("O bairro precisa ter 8 caracteres");

            RuleFor(c => c.Logradouro)
                .NotEmpty().WithMessage("O logradouro precisa ser fornecido")
                .Length(2, 150).WithMessage("O logradouro precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Numero)
                .NotEmpty().WithMessage("O número precisa ser fornecido")
                .Length(1, 10).WithMessage("O número precisa ter entre 1 e 10 caracteres");

            RuleFor(c => c.Bairro)
                .NotEmpty().WithMessage("O bairro precisa ser fornecido")
                .Length(2, 50).WithMessage("O bairro precisa ter entre 2 e 50 caracteres");

            RuleFor(c => c.Cidade)
                .NotEmpty().WithMessage("A cidade precisa ser fornecido")
                .Length(2, 50).WithMessage("A cidade precisa ter entre 2 e 50 caracteres");

            RuleFor(c => c.Estado)
                .NotEmpty().WithMessage("O Estado precisa ser fornecido")
                .Length(2).WithMessage("O Estado precisa ter 2 caracteres");
        }

        #endregion
    }
}
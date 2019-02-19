using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Organizadores;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos
{
    public class Evento : Entity<Evento>
    {
        protected Evento() { }

        public Evento(
            string nome,
            DateTime dataInicio,
            DateTime dataFim,
            bool gratuito,
            decimal valor,
            bool online,
            string nomeEmpresa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Gratuito = gratuito;
            Valor = valor;
            Online = online;
            NomeEmpresa = nomeEmpresa;
        }

        public string Nome { get; private set; }

        public string DescricaoCurta { get; private set; }

        public string DescricaoLonga { get; private set; }

        public DateTime DataInicio { get; private set; }

        public DateTime DataFim { get; private set; }

        public bool Gratuito { get; private set; }

        public decimal Valor { get; private set; }

        public bool Online { get; private set; }

        public string NomeEmpresa { get; private set; }

        public bool Excluido { get; private set; }

        public ICollection<Tags> Tags { get; private set; }

        public Guid? CategoriaId { get; private set; }

        public Guid? EnderecoId { get; private set; }

        public Guid OrganizadorId { get; private set; }


        // EF Propriedade de Navegação
        public virtual Categoria Categoria { get; private set; }

        public virtual Endereco Endereco { get; private set; }

        public virtual Organizador Organizador { get; private set; }


        // AddHoc Setters
        public void AtribuirEndereco(Endereco endereco)
        {
            if (!endereco.IsValid()) return;

            Endereco = endereco;
        }

        public void AtribuirCategoria(Categoria categoria)
        {
            if (!categoria.IsValid()) return;
            Categoria = categoria;
        }

        public void ExcluirEvento()
        {
            // TODO: deve validar alguma regra?

            Excluido = true;
        }


        public static class EventoFactory
        {
            public static Evento NovoEvento(Guid id, string nome, string descCurta, string descLonga, DateTime dataInicio, DateTime dataFim, bool gratuito, decimal valor, bool online, string nomeEmpresa, Guid? organizadorId, Endereco endereco, Guid categoriaId)
            {
                var evento = new Evento()
                {
                    Id = id,
                    Nome = nome,
                    DescricaoCurta = descCurta,
                    DescricaoLonga = descLonga,
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    Gratuito = gratuito,
                    Valor = valor,
                    Online = online,
                    NomeEmpresa = nomeEmpresa,
                    Endereco = endereco,
                    CategoriaId = categoriaId
                };

                if (organizadorId.HasValue)
                    evento.OrganizadorId = organizadorId.Value;

                if (online)
                    evento.Endereco = null;

                return evento;
            }
        }
        
        #region Validation

        public override bool IsValid()
        {
            ValidationRules();

            ValidationResult = Validate(this);

            // Validações adicionais
            ValidarEndereco();

            return ValidationResult.IsValid;
        }

        private void ValidationRules()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do evento precisa ser fornecido")
                .Length(2, 150).WithMessage("O nome do evento precisa ter entre 2 e 150 caracteres");

            // Valida se gratuito ou pago
            if (!Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(1, 50000)
                    .WithMessage("O valor deve estar entre 1.00 e 50.000");

            if (Gratuito)
                RuleFor(c => c.Valor)
                    .Equal(0).When(e => e.Gratuito)
                    .WithMessage("O valor não deve ser diferente de 0 para um evento gratuito");


            // Valida as datas
            RuleFor(c => c.DataInicio)
                .GreaterThan(DateTime.Now)
                .WithMessage("A data de início não de ser menor que a data atual");

            RuleFor(c => c.DataFim)
                .GreaterThanOrEqualTo(c => c.DataInicio)
                .WithMessage("A data final não deve ser menor que a data de início do evento");


            // Valida o local e organizador
            if (Online)
                RuleFor(c => c.Endereco)
                    .Null().When(c => c.Online)
                    .WithMessage("O evento não deve possuir um endereço se for online");

            if (!Online)
                RuleFor(c => c.Endereco)
                    .NotNull().When(c => c.Online == false)
                    .WithMessage("O evento deve possuir um endereço");

            RuleFor(c => c.NomeEmpresa)
                .NotEmpty().WithMessage("O nome do organizador precisa ser fornecido")
                .Length(2, 100).WithMessage("O nome do organizador precisa ter entre 2 e 100 caracteres");
        }

        /// <summary>
        /// Valida a entidade Endereco e adiciona os possíveis erros ao contexto desta entidade
        /// </summary>
        private void ValidarEndereco()
        {
            if (Online) return;
            if (Endereco.IsValid()) return;

            foreach (var error in Endereco.ValidationResult.Errors)
            {
                ValidationResult.Errors.Add(error);
            }
        }

        #endregion
    }
}

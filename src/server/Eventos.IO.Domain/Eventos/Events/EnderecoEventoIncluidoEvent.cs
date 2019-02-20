using Eventos.IO.Domain.Core.Events;
using System;

namespace Eventos.IO.Domain.Eventos.Events
{
    public class EnderecoEventoIncluidoEvent : Event
    {
        public Guid Id { get; private set; }
        public string Cep { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public Guid? EventoId { get; private set; }

        public EnderecoEventoIncluidoEvent(Guid enderecoId, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, Guid eventoId)
        {
            Id = enderecoId;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            EventoId = eventoId;
            AggregatedId = eventoId;
        }
    }
}

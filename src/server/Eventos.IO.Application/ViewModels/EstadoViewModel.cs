using System.Collections.Generic;

namespace Eventos.IO.Application.ViewModels
{
    public class EstadoViewModel
    {
        public string Sigla { get; set; }

        public string Nome { get; set; }

        public static List<EstadoViewModel> Listar()
        {
            return new List<EstadoViewModel>()
            {
                new EstadoViewModel() {Sigla = "AC", Nome = "Acre"},
                new EstadoViewModel() {Sigla = "AL", Nome = "Alagoas"},
                new EstadoViewModel() {Sigla = "AP", Nome = "Amapá"},
                new EstadoViewModel() {Sigla = "AM", Nome = "Amazonas"},
                new EstadoViewModel() {Sigla = "BA", Nome = "Bahia"},
                new EstadoViewModel() {Sigla = "CE", Nome = "Ceará"},
                new EstadoViewModel() {Sigla = "DF", Nome = "Distrito Federal"},
                new EstadoViewModel() {Sigla = "ES", Nome = "Espírito Santo"},
                new EstadoViewModel() {Sigla = "GO", Nome = "Goiás"},
                new EstadoViewModel() {Sigla = "MA", Nome = "Maranhão"},
                new EstadoViewModel() {Sigla = "MT", Nome = "Mato Grosso"},
                new EstadoViewModel() {Sigla = "MS", Nome = "Mato Grosso do Sul"},
                new EstadoViewModel() {Sigla = "MG", Nome = "Minas Gerais"},
                new EstadoViewModel() {Sigla = "PA", Nome = "Pará"},
                new EstadoViewModel() {Sigla = "PB", Nome = "Paraíba"},
                new EstadoViewModel() {Sigla = "PR", Nome = "Paraná"},
                new EstadoViewModel() {Sigla = "PE", Nome = "Pernambuco"},
                new EstadoViewModel() {Sigla = "PI", Nome = "Piauí"},
                new EstadoViewModel() {Sigla = "RJ", Nome = "Rio de Janeiro"},
                new EstadoViewModel() {Sigla = "RN", Nome = "Rio Grande do Norte"},
                new EstadoViewModel() {Sigla = "RS", Nome = "Rio Grande do Sul"},
                new EstadoViewModel() {Sigla = "RO", Nome = "Rondônia"},
                new EstadoViewModel() {Sigla = "RR", Nome = "Roraima"},
                new EstadoViewModel() {Sigla = "SC", Nome = "Santa Catarina"},
                new EstadoViewModel() {Sigla = "SP", Nome = "São Paulo"},
                new EstadoViewModel() {Sigla = "SE", Nome = "Sergipe"},
                new EstadoViewModel() {Sigla = "TO", Nome = "Tocantins"}
            };
        }
    }
}

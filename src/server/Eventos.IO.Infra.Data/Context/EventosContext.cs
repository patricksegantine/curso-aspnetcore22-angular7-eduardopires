using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Infra.Data.Context.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Eventos.IO.Infra.Data.Context
{
    public class EventosContext : DbContext
    {
        public EventosContext(DbContextOptions<EventosContext> options) : base(options) { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventoMap());
            modelBuilder.ApplyConfiguration(new EnderecoMap());
            modelBuilder.ApplyConfiguration(new OrganizadorMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());

            base.OnModelCreating(modelBuilder);
        }

    }
}

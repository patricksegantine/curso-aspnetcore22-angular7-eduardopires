using Eventos.IO.Domain.Eventos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventos.IO.Infra.Data.Context.Mappings
{
    public class EventoMap : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.Property(e => e.Nome).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.DescricaoCurta).HasColumnType("varchar(150)");
            builder.Property(e => e.DescricaoLonga).HasColumnType("varchar(1000)");
            builder.Property(e => e.NomeEmpresa).HasColumnType("varchar(100)");

            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.Tags);
            builder.Ignore(e => e.CascadeMode);
            builder.ToTable("Eventos");

            builder.HasOne(e => e.Organizador)
                      .WithMany(o => o.Eventos)
                      .HasForeignKey(e => e.OrganizadorId);

            builder.HasOne(e => e.Categoria)
                      .WithMany(e => e.Eventos)
                      .HasForeignKey(e => e.CategoriaId)
                      .IsRequired(false);
        }
    }
}

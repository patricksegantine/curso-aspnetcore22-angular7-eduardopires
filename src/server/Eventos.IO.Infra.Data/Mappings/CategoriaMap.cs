using Eventos.IO.Domain.Eventos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventos.IO.Infra.Data.Context.Mappings
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.Property(e => e.Nome)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Ignore(c => c.ValidationResult);

            builder.Ignore(c => c.CascadeMode);

            builder.ToTable("Categorias");
        }
    }
}

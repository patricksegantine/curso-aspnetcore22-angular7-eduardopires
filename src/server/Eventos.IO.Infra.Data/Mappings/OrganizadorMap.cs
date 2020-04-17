using Eventos.IO.Domain.Organizadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventos.IO.Infra.Data.Context.Mappings
{
    public class OrganizadorMap : IEntityTypeConfiguration<Organizador>
    {
        public void Configure(EntityTypeBuilder<Organizador> builder)
        {
            builder.Property(e => e.Nome)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(e => e.CpfCnpj)
                .HasColumnType("varchar(14)")
                .IsRequired();

            builder.Ignore(c => c.ValidationResult);

            builder.Ignore(c => c.CascadeMode);

            builder.ToTable("Organizadores");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReciboFacil.Dominio.Entidades;

namespace ReciboFacil.Repositorio.Configuracoes
{
    public class ClienteConfiguracoes : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Endereco).HasMaxLength(200);
            builder.Property(c => c.Bairro).HasMaxLength(100);
            builder.Property(c => c.Cidade).HasMaxLength(100);
            builder.Property(c => c.Telefone).HasMaxLength(20);
            builder.Property(c => c.CnpjCpf).HasMaxLength(20);
        }
    }
}

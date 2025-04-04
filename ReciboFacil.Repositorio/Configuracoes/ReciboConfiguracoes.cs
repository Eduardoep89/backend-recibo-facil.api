using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReciboFacil.Dominio.Entidades;

namespace ReciboFacil.Repositorio.Configuracoes
{
    public class ReciboConfiguracoes : IEntityTypeConfiguration<Recibo>
    {
        public void Configure(EntityTypeBuilder<Recibo> builder)
        {
            builder.ToTable("Recibos"); // Nome da tabela no banco de dados

            builder.HasKey(r => r.Id); // CHAVE P

            builder.Property(r => r.NumeroPedido)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.Data)
                .IsRequired();

            builder.Property(r => r.Descricao)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.Subtotal)
                .HasColumnType("decimal(18, 2)");

            builder.Property(r => r.Total)
                .HasColumnType("decimal(18, 2)");

            // Relacionamento com Cliente
            builder.HasOne(r => r.Cliente)
                .WithMany(c => c.Recibos)
                .HasForeignKey(r => r.ClienteId);

            // Relacionamento com ItensRecibo
            builder.HasMany(r => r.Itens)
                .WithOne(i => i.Recibo)
                .HasForeignKey(i => i.ReciboId);
        }
    }
}
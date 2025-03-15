using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReciboFacil.Dominio.Entidades;


namespace ReciboFacil.Repositorio.Configuracoes
{
public class ItemReciboConfiguracoes : IEntityTypeConfiguration<ItemRecibo>
{
    public void Configure(EntityTypeBuilder<ItemRecibo> builder)
    {
        builder.ToTable("ItensRecibo"); // Nome da tabela no banco de dados

        builder.HasKey(i => i.Id); // Chave primária

        builder.Property(i => i.Quantidade)
            .IsRequired();

        builder.Property(i => i.PrecoUnitario)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(i => i.Subtotal)
            .HasColumnType("decimal(18, 2)");

        // Relacionamento com Recibo
        builder.HasOne(i => i.Recibo)
            .WithMany(r => r.Itens)
            .HasForeignKey(i => i.ReciboId);

        // Relacionamento com Produto
        builder.HasOne(i => i.Produto)
            .WithMany()
            .HasForeignKey(i => i.ProdutoId);
    }
  }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReciboFacil.Dominio.Entidades;

namespace ReciboFacil.Repositorio.Configuracoes
{
    public class ProdutoConfiguracoes : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Marca).HasMaxLength(50);
            builder.Property(p => p.Modelo).HasMaxLength(50);
            builder.Property(p => p.Preco).HasColumnType("decimal(18,2)");

            // Configuração da chave estrangeira ClienteId
            builder.HasOne(p => p.Cliente)  // Relacionamento com Cliente
                .WithMany(c => c.Produtos)  // Um Cliente pode ter vários Produtos
                .HasForeignKey(p => p.ClienteId)  // Definindo ClienteId como chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Ao deletar um cliente, os produtos dele também serão deletados
        }
    }
}

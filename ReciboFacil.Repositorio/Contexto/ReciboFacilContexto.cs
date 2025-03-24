using Microsoft.EntityFrameworkCore;
using ReciboFacil.Dominio.Entidades;
using ReciboFacil.Repositorio.Configuracoes;

public class ReciboFacilContexto : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ProdutoPorCliente> ProdutosPorCliente { get; set; }
    public DbSet<Recibo> Recibos { get; set; }
    public DbSet<ItemRecibo> ItensRecibo { get; set; }

    public ReciboFacilContexto(DbContextOptions<ReciboFacilContexto> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica as configurações de Cliente e Produto
        modelBuilder.ApplyConfiguration(new ClienteConfiguracoes());
        modelBuilder.ApplyConfiguration(new ProdutoConfiguracoes());
        modelBuilder.ApplyConfiguration(new ReciboConfiguracoes());
        modelBuilder.ApplyConfiguration(new ItemReciboConfiguracoes());

        // Configuração do relacionamento entre Cliente e Produto
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Cliente)  // Cada Produto tem um Cliente
            .WithMany(c => c.Produtos)  // Um Cliente pode ter vários Produtos
            .HasForeignKey(p => p.ClienteId);  // A chave estrangeira em Produto é ClienteId

        // Configuração da entidade ProdutoPorCliente (resultado da função SQL)
        modelBuilder.Entity<ProdutoPorCliente>(entity =>
        {
            entity.HasNoKey(); // Funções com valor de tabela não têm chave primária
            entity.ToView("ListarProdutosPorCliente"); // Nome da função no banco de dados

            // Especifica a precisão e escala para a propriedade Preco
            entity.Property(p => p.Preco).HasPrecision(18, 2);
        });

        // Configuração da entidade ItemRecibo
        modelBuilder.Entity<ItemRecibo>(entity =>
        {
            // Especifica a precisão e escala para propriedades decimais
            entity.Property(ir => ir.PrecoUnitario).HasPrecision(18, 2);
            entity.Property(ir => ir.Subtotal).HasPrecision(18, 2);

            // Configuração das FOREIGN KEY com ON DELETE NO ACTION
            entity.HasOne(ir => ir.Recibo)
                .WithMany(r => r.Itens)
                .HasForeignKey(ir => ir.ReciboId)
                .OnDelete(DeleteBehavior.Restrict); // Evita ON DELETE CASCADE

            entity.HasOne(ir => ir.Produto)
                .WithMany(p => p.Itens)
                .HasForeignKey(ir => ir.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict); // Evita ON DELETE CASCADE
        });
    }
}
namespace ReciboFacil.Dominio.Entidades
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } // Nome do Produto
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; private set; } = true;

        // Relacionamento com Cliente - Cada produto pertence a um cliente
        public int ClienteId { get; set; }  // Chave estrangeira para Cliente
        public Cliente Cliente { get; set; }  // Referência ao Cliente
        // Propriedade de navegação para os itens de recibo associados a este produto
        public ICollection<ItemRecibo> Itens { get; set; } = new List<ItemRecibo>();

        public Produto() { }

        public void Deletar()
        {
            Ativo = false;
        }

        public void Restaurar()
        {
            Ativo = true;
        }
    }
}

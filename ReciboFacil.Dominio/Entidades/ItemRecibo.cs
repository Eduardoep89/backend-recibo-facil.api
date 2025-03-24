namespace ReciboFacil.Dominio.Entidades
{
    public class ItemRecibo
    {
        public int Id { get; set; }
        public int Quantidade { get; set; } // Quantidade do produto
        public decimal PrecoUnitario { get; set; } // Preço unitário do produto
        public decimal Subtotal { get; private set; } // Subtotal (Quantidade * PrecoUnitario)

        // Relacionamento com Recibo - Cada item pertence a um recibo
        public int ReciboId { get; set; } // Chave estrangeira para Recibo
        public Recibo Recibo { get; set; } // Referência ao Recibo

        // Relacionamento com Produto - Cada item está associado a um produto
        public int ProdutoId { get; set; } // Chave estrangeira para Produto
        public Produto Produto { get; set; } // Referência ao Produto

        public ItemRecibo() { }

        // Método para calcular o subtotal do item
        public void CalcularSubtotal()
        {
            Subtotal = Quantidade * PrecoUnitario;
        }
    }
}
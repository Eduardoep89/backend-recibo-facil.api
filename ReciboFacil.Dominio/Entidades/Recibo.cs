namespace ReciboFacil.Dominio.Entidades
{
    public class Recibo
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; } // Número do pedido (pode ser gerado automaticamente)
        public DateTime Data { get; set; } // Data do recibo
        public string Descricao { get; set; } // Descrição do serviço
        public decimal Subtotal { get; set; } // Subtotal (calculado automaticamente)
        public decimal Total { get; private set; } // Total (calculado automaticamente)
        public bool Ativo { get; set; } = true;

        // Relacionamento com Cliente - Cada recibo pertence a um cliente
        public int ClienteId { get; set; } // Chave estrangeira para Cliente
        public Cliente Cliente { get; set; } // Referência ao Cliente

        // Relacionamento com ItensRecibo - Um recibo pode ter vários itens
        public ICollection<ItemRecibo> Itens { get; set; } = new List<ItemRecibo>();

        public Recibo() { }

        // Método para calcular o subtotal e o total
    public void CalcularTotais()
    {
        foreach (var item in Itens)
        {
            item.CalcularSubtotal();
        }
        Subtotal = Itens.Sum(item => item.Subtotal);
        Total = Subtotal;
    }

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
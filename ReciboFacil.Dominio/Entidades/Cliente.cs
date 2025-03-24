namespace ReciboFacil.Dominio.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string CnpjCpf { get; set; }
        public bool Ativo { get; private set; } = true;

        // Relacionamento 1:N - Um cliente pode ter vários produtos
        public List<Produto> Produtos { get; set; }
          // Relacionamento 1:N - Um cliente pode ter vários recibos
        public List<Recibo> Recibos { get; set; }

        public Cliente() 
        {
            Produtos = new List<Produto>();
            Recibos = new List<Recibo>();
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

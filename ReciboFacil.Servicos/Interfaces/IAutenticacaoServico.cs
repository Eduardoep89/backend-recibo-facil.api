public interface IAutenticacaoServico
{
    Task<int> RegistrarAsync(string nome, string email, string senha);
    Task<bool> AutenticarAsync(string email, string senha);
    Task RegistrarSessaoAsync(string email);
    Task<bool> ValidarSessaoAsync();
    Task EncerrarSessaoAsync();
}
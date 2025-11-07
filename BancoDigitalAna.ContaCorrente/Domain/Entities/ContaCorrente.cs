namespace BancoDigitalAna.ContaCorrente.Domain.Entities
{
    public class ContaCorrente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo {  get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
    }
}

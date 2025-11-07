using BancoDigitalAna.ContaCorrente.Domain.ValueObjects;

namespace BancoDigitalAna.ContaCorrente.Domain.Entities
{
    public class ContaCorrente
    {
        public Guid Id { get; private set; }
        public Cpf Cpf { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public bool Ativo {  get; private set; }
        public string Senha { get; private set; }
        public string Salt { get; private set; }
    }
}

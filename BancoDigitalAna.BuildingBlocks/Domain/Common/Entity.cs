namespace BancoDigitalAna.BuildingBlocks.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AdicionarEvento(IDomainEvent evento)
        {
            _domainEvents.Add(evento);
        }

        public void LimparEventos()
        {
            _domainEvents.Clear();
        }

        public interface IAggregateRoot { }
    }
}

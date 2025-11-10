using BancoDigitalAna.BuildingBlocks.Domain.Common;

namespace BancoDigitalAna.Tarifacao.Domain.Entities
{
    public class Tarifas : Entity
    {
        public Guid ContaCorrenteId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataHora { get; private set; }

        private Tarifas (Guid id, Guid contaCorrenteId, decimal valor, DateTime dataHora)
        {
            Id = id;
            ContaCorrenteId = contaCorrenteId;
            Valor = valor;
            DataHora = dataHora;
        }

        public static Tarifas Criar (Guid contaCorrenteId, decimal valor)
        {
            return new Tarifas(Guid.NewGuid(), contaCorrenteId, valor, DateTime.UtcNow);
        }
    }
}

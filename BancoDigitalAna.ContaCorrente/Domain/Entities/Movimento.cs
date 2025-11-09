using BancoDigitalAna.BuildingBlocks.Domain.Common;
using BancoDigitalAna.Conta.Domain.ValueObjects;

namespace BancoDigitalAna.Conta.Domain.Entities
{
    public class Movimento : Entity
    {
        public Guid IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public TipoMovimento TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }

        private Movimento() { }

        private Movimento (Guid idContaCorrente, TipoMovimento tipoMovimento, decimal valor)
        {
            Id = new Guid();
            IdContaCorrente = idContaCorrente;
            TipoMovimento = tipoMovimento;
            Valor = valor;
            DataMovimento = DateTime.UtcNow;
        }

        public static Movimento Criar(Guid idContaCorrente, char tipo, decimal valor)
        {
            TipoMovimento tipoMovimento = TipoMovimento.Criar(tipo);

            return new Movimento(idContaCorrente, tipoMovimento, valor);
        }
    }
}

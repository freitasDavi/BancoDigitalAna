using BancoDigitalAna.BuildingBlocks.Domain.Common;
using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;

namespace BancoDigitalAna.Transferencia.Domain.Entities
{
    public class Transferencias : Entity
    {
        public Guid ContaOrigem { get; private set; }
        public Guid ContaDestino { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataMovimento { get; private set; }

        private Transferencias (Guid idTransferencia, Guid contaOrigem, Guid contaDestino, decimal valor, DateTime dataMovimento)
        {
            Id = idTransferencia;
            ContaOrigem = contaOrigem;
            ContaDestino = contaDestino;
            Valor = valor;
            DataMovimento = dataMovimento;
        }

        public static Transferencias Criar (Guid contaOrigem, Guid contaDestino, decimal valor)
        {
            if (valor <= 0)
                throw new DomainException("Valor da trasnferência deve ser maior que zero", "INVALID_VALUE");

            return new Transferencias(Guid.NewGuid(), contaOrigem, contaDestino, valor, DateTime.UtcNow);
        }

    }
}

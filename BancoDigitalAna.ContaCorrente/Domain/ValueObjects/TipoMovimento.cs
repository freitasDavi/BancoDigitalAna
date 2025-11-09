using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;

namespace BancoDigitalAna.Conta.Domain.ValueObjects
{
    public class TipoMovimento
    {
        public char Codigo { get; private set; }
        public string Descricao => GetDescricao();
    
        protected TipoMovimento () { }

        private TipoMovimento(char codigo)
        {
            Codigo = codigo;
        }

        public static TipoMovimento Criar (char codigo)
        {
            ValidarCodigoDeMovimento(codigo);

            return new TipoMovimento(codigo);
        }

        public static void ValidarCodigoDeMovimento (char codigo)
        {
            var codigosValidos = new[] { 'C', 'D' };

            if (!codigosValidos.Contains(codigo))
                throw new DomainException("Por favor insira um Tipo de Movimento válido", "INVALID_TYPE");
        }

        public string GetDescricao ()
        {
            // Caso haja novas opções somente adicionar no switch
            switch (Codigo) {
                case 'D':
                    return "Débito";
                default: 
                    return "Crédito";
            }
        }
    }
}

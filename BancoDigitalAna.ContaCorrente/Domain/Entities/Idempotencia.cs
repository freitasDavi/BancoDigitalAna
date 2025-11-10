namespace BancoDigitalAna.Conta.Domain.Entities
{
    public class Idempotencia
    {
        public Guid ChaveRequisicao { get; private set; }    
        public string Requisicao { get; private set; } = string.Empty;
        public string Resultado { get; private set; } = string.Empty;

        private Idempotencia(Guid chaveRequisicao, string requisicao, string resultado) 
        {
            ChaveRequisicao = chaveRequisicao;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public static Idempotencia Criar (Guid chaveRequisicao, string requisicao, string resultado)
        {
            return new Idempotencia(chaveRequisicao, requisicao, resultado);
        }
    }
}

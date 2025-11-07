namespace BancoDigitalAna.ContaCorrente.Domain.Entities
{
    public class Movimento
    {
        public Guid IdMovimento { get; set; }
        public Guid IdContaCorrent { get; set; }
        public DateTime DataMovimento { get; set; }
        public string TipoMovimento { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}

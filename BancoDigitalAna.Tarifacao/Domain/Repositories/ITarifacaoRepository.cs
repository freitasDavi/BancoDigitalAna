using BancoDigitalAna.Tarifacao.Domain.Entities;

namespace BancoDigitalAna.Tarifacao.Domain.Repositories
{
    public interface ITarifacaoRepository
    {
        Task Inserir(Tarifas tarifa);
    }
}

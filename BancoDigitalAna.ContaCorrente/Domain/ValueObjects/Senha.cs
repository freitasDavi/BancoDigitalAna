using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace BancoDigitalAna.Conta.Domain.ValueObjects
{
    public class Senha
    {
        public string Hash { get; private set; }
        public string Salt { get; private set; }

        public Senha(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }

        public static Senha Criar(string senhaTexto)
        {
            if (string.IsNullOrWhiteSpace(senhaTexto) || senhaTexto.Length < 6)
                throw new DomainException("Senha deve ter no mínimo 6 caractéres", "INVALID_PASSWORD");

            var salt = GerarSalt();
            var hash = GerarHash(senhaTexto, salt);

            return new Senha(hash, salt);
        }

        private static string GerarHash(string senhaTexto, string salt)
        {
            using var sha256 = SHA256.Create();

            var combined = Encoding.UTF8.GetBytes(senhaTexto + salt);

            var hash = sha256.ComputeHash(combined);

            return Convert.ToBase64String(hash);
        }

        private static string GerarSalt()
        {
            var bytes = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}

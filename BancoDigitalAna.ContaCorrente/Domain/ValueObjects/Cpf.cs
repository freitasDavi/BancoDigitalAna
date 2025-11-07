using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace BancoDigitalAna.ContaCorrente.Domain.ValueObjects
{
    public class Cpf
    {
        public string Numero {  get; private set; }

        private Cpf (String numero)
        {
            Numero = numero;
        }

        public static Cpf Criar (string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new DomainException("O campo CPF não pode ser vazio", "INVALID_DOCUMENT");

            var cpfLimpo = Regex.Replace(cpf, @"[^\d]", "");

            if (!ValidarCpf(cpfLimpo))
                throw new DomainException("CPF Inválido", "INVALID_DOCUMENT");

            return new Cpf(cpfLimpo);
        }

        private static bool ValidarCpf (string cpf)
        {
            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
                return false;

            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var cpfTemporario = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            var digito = resto.ToString();
            cpfTemporario += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public override int GetHashCode() => Numero.GetHashCode();

        public static implicit operator string(Cpf cpf) => cpf.Numero;
    }
}

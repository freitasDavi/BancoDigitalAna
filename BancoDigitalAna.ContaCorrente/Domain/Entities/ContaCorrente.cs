using BancoDigitalAna.BuildingBlocks.Domain.Common;
using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.Conta.Domain.Events;
using BancoDigitalAna.Conta.Domain.ValueObjects;

namespace BancoDigitalAna.Conta.Domain.Entities
{
    public class ContaCorrente : Entity
    {
        public Cpf Cpf { get; private set; }
        public int NumeroConta { get; private set; }
        public string NomeTitular { get; private set; }
        public bool Ativo {  get; private set; }
        public Senha Senha { get; private set; }

        private readonly List<Movimento> _movimentos = new();
        public IReadOnlyCollection<Movimento> Movimentos => _movimentos.AsReadOnly();

        private ContaCorrente() { }

        private ContaCorrente(Cpf cpf, string nomeTitular, Senha senha)
        {
            Id = Guid.NewGuid();
            Cpf = cpf;
            NomeTitular = nomeTitular;
            Senha = senha;
            NumeroConta = GerarNumeroConta();
            Ativo = true;

            AdicionarEvento(new ContaCorrenteCriadaEvent(Id, NumeroConta, cpf));
        }

        public static ContaCorrente Criar(string cpf, string nomeTitular, string senha)
        {
            var cpfVO = Cpf.Criar(cpf);
            var senhaVO = Senha.Criar(senha);

            return new ContaCorrente(cpfVO, nomeTitular, senhaVO);
        }
        
        public void Inativar(string senhaInput)
        {
            if (!Ativo)
                throw new DomainException("Conta já esta inativa", "INACTIVE_ACCOUNT");

            if (!Senha.Validar(senhaInput))
                throw new DomainException("Senha não inválida", "USER_UNAUTHORIZED");

            Ativo = false;

            //AdicionarEvento(new )
        }

        public void ContaEstaAtiva()
        {
            ValidarConta("ser alvos de consulta");
        }

        public int GerarNumeroConta()
        {
            return new Random().Next(10000000, 99999999);
        }
    
        public void AdicionarMovimento(char tipo, decimal valor,string? numeroConta)
        {
            if (!string.IsNullOrEmpty(numeroConta) &&
                (int.Parse(numeroConta) != NumeroConta && tipo == 'D'))
                throw new DomainException("Caso a conta destino seja diferente da origem, o tipo deve ser Crédito", "INVALID_TYPE");

            ValidarConta("receber movimentação");

            if (valor <= 0)
                throw new DomainException("Valor deve ser positivo", "INVALID_VALUE");

            var movimento = Movimento.Criar(Id, tipo, valor);
            _movimentos.Add(movimento);
        }

        private void ValidarConta (string operacao)
        {
            if (!Ativo)
                throw new DomainException($"Contas inativas não podem {operacao}", "INACTIVE_ACCOUNT");
        }

        public decimal ConsultarSaldo()
        {
            ValidarConta("consultar saldo");

            decimal saldo = 0;
            _movimentos.ForEach((m) =>
            {
                if (m.TipoMovimento.Codigo == 'C')
                {
                    saldo += m.Valor;
                } else
                {
                    saldo -= m.Valor;
                }
            });

            return saldo;
        }
    }
}

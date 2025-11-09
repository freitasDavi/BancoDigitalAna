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

        public int GerarNumeroConta()
        {
            return new Random().Next(10000000, 99999999);
        }
    }
}

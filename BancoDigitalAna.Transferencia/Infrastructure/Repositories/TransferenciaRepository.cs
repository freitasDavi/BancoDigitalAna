using BancoDigitalAna.Transferencia.Domain.Entities;
using BancoDigitalAna.Transferencia.Domain.Repositories;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BancoDigitalAna.Transferencia.Infrastructure.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository 
    {
        private readonly string _connectionString;

        public TransferenciaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        private IDbConnection CreateConnection ()
        {
            return new OracleConnection(_connectionString);
        }

        public async Task<Guid> InserirAsync(Transferencias transferencia)
        {
            using var connection = CreateConnection();

            var sql = @"
                INSERT INTO TRANSFERENCIA (
                    IDTRANSFERENCIA,
                    IDCONTACORRENTE_ORIGEM,
                    IDCONTACORRENTE_DESTINO,
                    DATAMOVIMENTO,
                    VALOR
                ) VALUES (
                    :Id,
                    :IdContaCorrenteOrigem,
                    :IdContaCorrenteDestino,
                    :DataMovimento,
                    :Valor
                )";

            await connection.ExecuteAsync(sql, new
            {
                Id = transferencia.Id.ToString(),
                IdContaCorrenteOrigem = transferencia.ContaOrigem.ToString(),
                IdContaCorrenteDestino = transferencia.ContaDestino.ToString(),
                DataMovimento = transferencia.DataMovimento.ToString("dd/MM/yyyy HH:mm:ss"),
                Valor = transferencia.Valor
            });

            return transferencia.Id;
        }

        public async Task<Transferencias?> ObterPorIdRequisicaoAsync(Guid idRequisicao)
        {
            using var connection = CreateConnection();

            var sql = @"
                SELECT 
                    IDTRANSFERENCIA as Id,
                    IDCONTACORRENTE_ORIGEM as IdContaCorrenteOrigem,
                    IDCONTACORRENTE_DESTINO as IdContaCorrenteDestino,
                    DATAMOVIMENTO as DataMovimento,
                    VALOR as Valor
                FROM
                    TRANSFERENCIA
                WHERE
                    IDTRANSFERENCIA = :IdTransferencia
            ";

            var result = await connection.QueryFirstOrDefaultAsync<Transferencias>(
                sql,
                new { IdTransferencia = idRequisicao.ToString() }    
            );

            if (result == null)
                return null;

            return result;
        }
    }
}

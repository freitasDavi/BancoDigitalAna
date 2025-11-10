using BancoDigitalAna.Transferencia.Domain.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Dapper;

namespace BancoDigitalAna.Transferencia.Infrastructure.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly string _connectionString;
        public IdempotenciaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnetion ()
        {
            return new OracleConnection(_connectionString);            
        }

        public async Task<string?> RecuperarIdempotencia(string chaveIdempotencia)
        {
            using var connection = CreateConnetion();

            var sql = @"
                SELECT RESULTADO
                FROM IDEMPOTENCIA
                WHERE CHAVE_IDEMPOTENCIA = :ChaveIdempotencia
            ";

            return await connection.QueryFirstOrDefaultAsync<string>(
                sql,
                new { ChaveIdempotencia = chaveIdempotencia });
        }

        public async Task SalvarAsync(string chaveIdempotencia, string requisicao, string resultado)
        {
            using var connection = CreateConnetion();

            var sql = @"
                INSERT INTO IDEMPOTENCIA (
                    CHAVE_IDEMPOTENCIA,
                    REQUISICAO,
                    RESULTADO,
                ) VALUES (
                    :ChaveIdempotencia,
                    :Requisicao,
                    :Resultado
                );
            ";

            await connection.ExecuteAsync(sql, new
            {
                ChaveIdempotencia = chaveIdempotencia,
                Requisicao = requisicao,
                Resultado = resultado
            });
        }
    }
}

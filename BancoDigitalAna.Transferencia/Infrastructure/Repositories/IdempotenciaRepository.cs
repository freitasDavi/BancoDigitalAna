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

        public async Task<string?> RecuperarIdempotencia(Guid chaveIdempotencia)
        {
            using var connection = CreateConnetion();

            var sql = @"
                SELECT RESULTADO
                FROM IDEMPOTENCIA
                WHERE CHAVE_IDEMPOTENCIA = :ChaveIdempotencia
            ";

            return await connection.QueryFirstOrDefaultAsync<string>(
                sql,
                new { ChaveIdempotencia = chaveIdempotencia.ToString() });
        }

        public async Task SalvarAsync(Guid chaveIdempotencia, string requisicao, string resultado)
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
                ChaveIdempotencia = chaveIdempotencia.ToString(),
                Requisicao = requisicao,
                Resultado = resultado
            });
        }
    }
}

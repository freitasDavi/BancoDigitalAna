using BancoDigitalAna.Tarifacao.Domain.Entities;
using BancoDigitalAna.Tarifacao.Domain.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BancoDigitalAna.Tarifacao.Infrastructure
{
    public class TarifacaoRepository : ITarifacaoRepository
    {
        private readonly string _connectionString;

        public TarifacaoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection()
        {
            return new OracleConnection(_connectionString);
        }

        public async Task Inserir(Tarifas tarifa)
        {
            using var connection = CreateConnection();

            var sql = @"
                INSERT INTO TARIFA (
                    IDTARIFA,
                    IDCONTACORRENTE,
                    VALOR,
                    DATAMOVIMENTO
                ) VALUES (
                    :Id,
                    :ContaCorrenteId,
                    :Valor,
                    :DataMovimento
                )";

            await connection.ExecuteAsync(sql, new
            {
                Id = tarifa.Id.ToString(),
                ContaCorrenteId = tarifa.ContaCorrenteId.ToString(),
                tarifa.Valor,
                DataMovimento = tarifa.DataHora.ToString("dd/MM/yyyy")
            });
        }
    }
}

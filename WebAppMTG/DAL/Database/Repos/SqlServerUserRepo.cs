using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMTGModelsDL.Database.Models;
using WebAppMTGModelsDL.Exceptions;
using WebAppMTGModelsDL.Database.Interfaces;

namespace WebAppMTGDAL.Database.Repos
{
    public class SqlServerUserRepo : IUserRepo
    {
        private readonly string _connectionString;

        public SqlServerUserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserRecord?> GetUserByIdAsync(int id)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT id, name, email, password_hash
                FROM users
                WHERE id = @id;
                ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new UserRecord
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("password_hash"))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new DatabaseUnavailableException("De database is momenteel niet beschikbaar.", ex);
            }
        }

        public async Task<UserRecord?> GetUserByEmailAsync(string email)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT id, name, email, password_hash
                FROM users
                WHERE email = @email;
                ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new UserRecord
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("password_hash"))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new DatabaseUnavailableException("De database is momenteel niet beschikbaar.", ex);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT COUNT(*)
                FROM users
                WHERE email = @email;
                ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@email", email);

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch (SqlException ex)
            {
                throw new DatabaseUnavailableException("De database is momenteel niet beschikbaar.", ex);
            }
        }

        public async Task<int> CreateUserAsync(string name, string email, string passwordHash)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                INSERT INTO users (name, email, password_hash)
                VALUES (@name, @email, @passwordHash);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new DatabaseUnavailableException("De database is momenteel niet beschikbaar.", ex);
            }
        }
    }
}

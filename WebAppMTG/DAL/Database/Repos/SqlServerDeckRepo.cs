using System;
using System.Collections.Generic;
using System.Text;
using ModelsDL.Database.Models;
using MySqlConnector;
using WebAppMTGLogic.Interfaces;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

namespace WebAppMTGDAL.Database.Repos
{
    public class SqlServerDeckRepo : IMySQLDeckRepo
    {
        private readonly string _connectionString;

        public SqlServerDeckRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<DeckRecord>> GetDecksByUserIdAsync(int userId)
        {
            var result = new List<DeckRecord>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT i.id, i.user_id, i.name, d.format, d.description
                FROM items i
                INNER JOIN decks d ON d.item_id = i.id
                WHERE i.user_id = @userId
                  AND i.item_type = 'deck'
                ORDER BY i.name;
            ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userId", userId);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new DeckRecord
                    {
                        ItemId = reader.GetInt32(reader.GetOrdinal("id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Format = reader.GetString(reader.GetOrdinal("format")),
                        Description = reader.GetString(reader.GetOrdinal("description"))
                    });
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("De SQL Server database is niet bereikbaar of de query is mislukt.", ex);
            }
        }

        public async Task<DeckRecord?> GetDeckRecordByIdAsync(int itemId)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT i.id, i.user_id, i.name, d.format, d.description
                FROM items i
                INNER JOIN decks d ON d.item_id = i.id
                WHERE i.id = @itemId
                  AND i.item_type = 'deck';
            ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@itemId", itemId);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new DeckRecord
                    {
                        ItemId = reader.GetInt32(reader.GetOrdinal("id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Format = reader.GetString(reader.GetOrdinal("format")),
                        Description = reader.GetString(reader.GetOrdinal("description"))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("De SQL Server database is niet bereikbaar of de query is mislukt.", ex);
            }
        }

        public async Task<List<DeckCardEntry>> GetDeckEntriesByItemIdAsync(int itemId)
        {
            var result = new List<DeckCardEntry>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT id, item_id, card_id, quantity, boardpart
                FROM item_card_references
                WHERE item_id = @itemId
                ORDER BY boardpart, id;
            ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@itemId", itemId);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new DeckCardEntry
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                        CardId = reader.GetString(reader.GetOrdinal("card_id")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                        BoardPart = ParseBoardPart(reader.GetString(reader.GetOrdinal("boardpart")))
                    });
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("De SQL Server database is niet bereikbaar of de query is mislukt.", ex);
            }
        }

        public async Task<int> CreateDeckAsync(int userId, string name, string format, string description)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

                try
                {
                    int itemId;

                    const string itemSql = @"
                    INSERT INTO items (user_id, name, item_type)
                    VALUES (@userId, @name, 'deck');
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                    await using (var itemCmd = new SqlCommand(itemSql, connection, transaction))
                    {
                        itemCmd.Parameters.AddWithValue("@userId", userId);
                        itemCmd.Parameters.AddWithValue("@name", name);

                        var result = await itemCmd.ExecuteScalarAsync();
                        itemId = Convert.ToInt32(result);
                    }

                    const string deckSql = @"
                    INSERT INTO decks (item_id, format, description)
                    VALUES (@itemId, @format, @description);
                ";

                    await using (var deckCmd = new SqlCommand(deckSql, connection, transaction))
                    {
                        deckCmd.Parameters.AddWithValue("@itemId", itemId);
                        deckCmd.Parameters.AddWithValue("@format", format);
                        deckCmd.Parameters.AddWithValue("@description", description);

                        await deckCmd.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                    return itemId;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("De SQL Server database is niet bereikbaar of de query is mislukt.", ex);
            }
        }

        public async Task<int> AddCardToDeckAsync(int itemId, string cardId, int quantity, BoardPart boardPart)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                INSERT INTO item_card_references (item_id, card_id, quantity, boardpart)
                VALUES (@itemId, @cardId, @quantity, @boardpart);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@itemId", itemId);
                cmd.Parameters.AddWithValue("@cardId", cardId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@boardpart", boardPart.ToString());

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("De SQL Server database is niet bereikbaar of de query is mislukt.", ex);
            }
        }

        public async Task RemoveCardFromDeckAsync(int itemId, string cardEntryId)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                DELETE FROM item_card_references
                WHERE card_id = @cardId AND item_id = @itemId;
            ";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@cardId", cardEntryId);
                cmd.Parameters.AddWithValue("@itemId", itemId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("De SQL Server database is niet bereikbaar of de query is mislukt.", ex);
            }
        }

        private static BoardPart ParseBoardPart(string dbValue)
        {
            return Enum.TryParse<BoardPart>(dbValue, true, out var result)
                ? result
                : BoardPart.Mainboard;
        }
    }
}


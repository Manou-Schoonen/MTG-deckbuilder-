using System;
using System.Collections.Generic;
using System.Text;
using ModelsDL.Database.Models;
using MySqlConnector;
using WebAppMTGLogic.Interfaces;

namespace WebAppMTGDAL.Database.Repos
{
    public class MySQLDeckRepo :IMySQLDeckRepo 
    {
        private readonly string _connectionString;

        public MySQLDeckRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<DeckRecord>> GetDecksByUserIdAsync(int userId)
        {
            var result = new List<DeckRecord>();

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
            SELECT i.id, i.user_id, i.name, d.format, d.description
            FROM items i
            INNER JOIN decks d ON d.item_id = i.id
            WHERE i.user_id = @userId
              AND i.item_type = 'deck'
            ORDER BY i.name;
        ";

            await using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@userId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new DeckRecord
                {
                    ItemId = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    Name = reader.GetString("name"),
                    Format = reader.GetString("format"),
                    Description = reader.GetString("description")
                });
            }

            return result;
        }

        public async Task<DeckRecord?> GetDeckRecordByIdAsync(int itemId)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
            SELECT i.id, i.user_id, i.name, d.format, d.description
            FROM items i
            INNER JOIN decks d ON d.item_id = i.id
            WHERE i.id = @itemId
              AND i.item_type = 'deck';
        ";

            await using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@itemId", itemId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DeckRecord
                {
                    ItemId = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    Name = reader.GetString("name"),
                    Format = reader.GetString("format"),
                    Description = reader.GetString("description")
                };
            }

            return null;
        }

        public async Task<List<DeckCardEntry>> GetDeckEntriesByItemIdAsync(int itemId)
        {
            var result = new List<DeckCardEntry>();

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
            SELECT id, item_id, card_id, quantity, boardpart
            FROM item_card_references
            WHERE item_id = @itemId
            ORDER BY boardpart, id;
        ";

            await using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@itemId", itemId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new DeckCardEntry
                {
                    Id = reader.GetInt32("id"),
                    ItemId = reader.GetInt32("item_id"),
                    CardId = reader.GetString("card_id"),
                    Quantity = reader.GetInt32("quantity"),
                    BoardPart = ParseBoardPart(reader.GetString("boardpart"))
                });
            }

            return result;
        }

        public async Task<int> CreateDeckAsync(int userId, string name, string format, string description)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                int itemId;

                const string itemSql = @"
                INSERT INTO items (user_id, name, item_type)
                VALUES (@userId, @name, 'deck');
                SELECT LAST_INSERT_ID();
            ";

                await using (var itemCmd = new MySqlCommand(itemSql, connection, (MySqlTransaction)transaction))
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

                await using (var deckCmd = new MySqlCommand(deckSql, connection, (MySqlTransaction)transaction))
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

        public async Task<int> AddCardToDeckAsync(int itemId, string cardId, int quantity, BoardPart boardPart)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
            INSERT INTO item_card_references (item_id, card_id, quantity, boardpart)
            VALUES (@itemId, @cardId, @quantity, @boardpart);
            SELECT LAST_INSERT_ID();
        ";

            await using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@cardId", cardId);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@boardpart", boardPart.ToString());

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task RemoveCardFromDeckAsync(int itemId, string cardId)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
        DELETE FROM item_card_references
        WHERE card_id = @cardId AND item_id = @itemId;
    ";

            await using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@cardId", cardId);
            cmd.Parameters.AddWithValue("@itemId", itemId);

            await cmd.ExecuteNonQueryAsync();
        }

        private static BoardPart ParseBoardPart(string dbValue)
        {
            return Enum.TryParse<BoardPart>(dbValue, true, out var result)
                ? result
                : BoardPart.Mainboard;
        }
    }
}


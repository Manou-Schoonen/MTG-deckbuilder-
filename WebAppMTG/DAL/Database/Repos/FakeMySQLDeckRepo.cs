using ModelsDL.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Interfaces;

namespace WebAppMTGDAL.Database.Repos
{
    public class FakeMySQLDeckRepo : IMySQLDeckRepo
    {
        public List<DeckRecord> DeckRecords { get; set; } = new();
        public Dictionary<int, List<DeckCardEntry>> DeckEntriesByItemId { get; set; } = new();

        public int CreateDeckResult { get; set; }

        public bool AddCardToDeckWasCalled { get; private set; }
        public int AddCardToDeck_ItemId { get; private set; }
        public string AddCardToDeck_CardId { get; private set; } = string.Empty;
        public int AddCardToDeck_Quantity { get; private set; }
        public BoardPart AddCardToDeck_BoardPart { get; private set; }

        public bool RemoveCardFromDeckWasCalled { get; private set; }
        public int RemoveCardFromDeck_ItemId { get; private set; }
        public string RemoveCardFromDeck_CardEntryId { get; private set; } = string.Empty;

        public int AddCardToDeckResult { get; set; }

        public Task<List<DeckRecord>> GetDecksByUserIdAsync(int userId)
        {
            var result = DeckRecords
                .Where(d => d.UserId == userId)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<DeckRecord?> GetDeckRecordByIdAsync(int itemId)
        {
            var result = DeckRecords.FirstOrDefault(d => d.ItemId == itemId);
            return Task.FromResult(result);
        }

        public Task<List<DeckCardEntry>> GetDeckEntriesByItemIdAsync(int itemId)
        {
            if (DeckEntriesByItemId.TryGetValue(itemId, out var entries))
                return Task.FromResult(entries);

            return Task.FromResult(new List<DeckCardEntry>());
        }

        public Task<int> CreateDeckAsync(int userId, string name, string format, string description)
        {
            return Task.FromResult(CreateDeckResult);
        }

        public Task<int> AddCardToDeckAsync(int itemId, string cardId, int quantity, BoardPart boardPart)
        {
            AddCardToDeckWasCalled = true;
            AddCardToDeck_ItemId = itemId;
            AddCardToDeck_CardId = cardId;
            AddCardToDeck_Quantity = quantity;
            AddCardToDeck_BoardPart = boardPart;

            return Task.FromResult(AddCardToDeckResult);
        }

        public Task RemoveCardFromDeckAsync(int itemId, string cardEntryId)
        {
            RemoveCardFromDeckWasCalled = true;
            RemoveCardFromDeck_ItemId = itemId;
            RemoveCardFromDeck_CardEntryId = cardEntryId;

            return Task.CompletedTask;
        }
    }
}

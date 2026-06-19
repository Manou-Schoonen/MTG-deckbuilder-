using ModelsDL.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Interfaces;

namespace UnitTests.test.IntergratieTest.FakeServices
{
    public class InMemoryDeckRepo : IDeckRepo
    {
        public List<DeckRecord> Decks { get; } = new();
        public List<DeckCardEntry> Entries { get; } = new();

        public Task<List<DeckRecord>> GetDecksByUserIdAsync(int userId)
        {
            return Task.FromResult(Decks.Where(d => d.UserId == userId).ToList());
        }

        public Task<DeckRecord?> GetDeckRecordByIdAsync(int itemId)
        {
            return Task.FromResult(Decks.FirstOrDefault(d => d.ItemId == itemId));
        }

        public Task<List<DeckCardEntry>> GetDeckEntriesByItemIdAsync(int itemId)
        {
            return Task.FromResult(Entries.Where(e => e.ItemId == itemId).ToList());
        }

        public Task<int> CreateDeckAsync(int userId, string name, string format, string description)
        {
            var nextId = Decks.Count == 0 ? 1 : Decks.Max(d => d.ItemId) + 1;

            Decks.Add(new DeckRecord
            {
                ItemId = nextId,
                UserId = userId,
                Name = name,
                Format = format,
                Description = description
            });

            return Task.FromResult(nextId);
        }

        public Task<int> AddCardToDeckAsync(int itemId, string cardId, int quantity, BoardPart boardPart)
        {
            var nextId = Entries.Count == 0 ? 1 : Entries.Max(e => e.Id) + 1;

            Entries.Add(new DeckCardEntry
            {
                Id = nextId,
                ItemId = itemId,
                CardId = cardId,
                Quantity = quantity,
                BoardPart = boardPart
            });

            return Task.FromResult(nextId);
        }

        public Task RemoveCardFromDeckAsync(int itemId, string cardId)
        {
            var entry = Entries.FirstOrDefault(e => e.ItemId == itemId && e.CardId == cardId);
            if (entry != null)
            {
                Entries.Remove(entry);
            }

            return Task.CompletedTask;
        }

        public Task UpdateDeckNameAsync(int itemId, string newName)
        {
            var deck = Decks.FirstOrDefault(d => d.ItemId == itemId);
            if (deck != null)
            {
                deck.Name = newName;
            }

            return Task.CompletedTask;
        }

        public Task DeleteDeckAsync(int itemId)
        {
            var deck = Decks.FirstOrDefault(d => d.ItemId == itemId);
            if (deck != null)
            {
                Decks.Remove(deck);
            }

            var entriesToRemove = Entries.Where(e => e.ItemId == itemId).ToList();
            foreach (var entry in entriesToRemove)
            {
                Entries.Remove(entry);
            }

            return Task.CompletedTask;
        }
    }
}

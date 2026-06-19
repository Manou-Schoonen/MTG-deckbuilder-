using ModelsDL.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppMTGLogic.Interfaces
{
    public interface IDeckRepo
    {
        Task<List<DeckRecord>> GetDecksByUserIdAsync(int userId);
        Task<DeckRecord?> GetDeckRecordByIdAsync(int itemId);

        Task<List<DeckCardEntry>> GetDeckEntriesByItemIdAsync(int itemId);
        Task<int> CreateDeckAsync(int userId, string name, string format, string description);

        Task<int> AddCardToDeckAsync(int itemId, string cardId, int quantity, BoardPart boardPart);
        Task RemoveCardFromDeckAsync(int itemId, string cardEntryId);
        Task UpdateDeckNameAsync(int itemId, string newName);
        Task DeleteDeckAsync(int itemId);
    }
}

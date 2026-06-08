using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Interfaces
{
    public interface IMySQLDeckRepo
    {
        Task<List<DeckRecord>> GetDecksByUserIdAsync(int userId);
        Task<DeckRecord?> GetDeckRecordByIdAsync(int itemId);

        Task<List<DeckCardEntry>> GetDeckEntriesByItemIdAsync(int itemId);
        Task<int> CreateDeckAsync(int userId, string name, string format, string description);

        Task<int> AddCardToDeckAsync(int itemId, string cardId, int quantity, BoardPart boardPart);
        Task RemoveCardFromDeckAsync(int itemId, int cardEntryId);
    }
}

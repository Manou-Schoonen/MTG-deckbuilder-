using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Interfaces
{
    public interface IUserDeckService
    {
        Task<List<DeckModel>> GetDecksByUserIdAsync(int userId);
        Task<DeckModel?> GetDeckByIdAsync(int itemId);
        Task<int> CreateDeckAsync(int userId, string name, string format, string description);
        Task AddCardToDeckAsync(int userId, int itemId, string cardId, int quantity, BoardPart boardPart);
        Task RemoveCardFromDeckAsync(int userId, int itemId, string cardEntryId);
        Task<DeckModel> BuildDeckAsync(IEnumerable<DeckCardEntry> entries);

        Task<DeckLegalityResult> ValidateDeckAsync(int itemId);
    }
}

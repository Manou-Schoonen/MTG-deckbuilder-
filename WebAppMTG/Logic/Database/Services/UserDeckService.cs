using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.Interfaces;
using ModelsDL.Database.Models;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;
using WebAppMTGLogic.FormatRules.Interface;


namespace WebAppMTGLogic.Database.Services
{
    public class UserDeckService : IUserDeckService
    {
        private readonly IMySQLDeckRepo _mysqlDeckRepo;
        private readonly ICardLogicService _cardLogicService;
        private readonly ILegalityRule _legalityRule;

        public UserDeckService(
            IMySQLDeckRepo mysqlDeckRepo,
            ICardLogicService cardLogicService,
            ILegalityRule legalityRule)
        {
            _mysqlDeckRepo = mysqlDeckRepo;
            _cardLogicService = cardLogicService;
            _legalityRule = legalityRule;
        }


        public async Task<List<DeckModel>> GetDecksByUserIdAsync(int userId)
        {
            var deckRecords = await _mysqlDeckRepo.GetDecksByUserIdAsync(userId);
            var result = new List<DeckModel>();

            foreach (var record in deckRecords)
            {
                var entries = await _mysqlDeckRepo.GetDeckEntriesByItemIdAsync(record.ItemId);
                var deckModel = await BuildDeckAsync(entries);

                deckModel.ItemId = record.ItemId.ToString();
                deckModel.UserId = record.UserId.ToString();
                deckModel.Name = record.Name;
                deckModel.Format = record.Format;
                deckModel.Description = record.Description;

                result.Add(deckModel);
            }

            return result;
        }

        public async Task<DeckModel?> GetDeckByIdAsync(int itemId)
        {
            var deckRecord = await _mysqlDeckRepo.GetDeckRecordByIdAsync(itemId);
            if (deckRecord == null)
                return null;

            var entries = await _mysqlDeckRepo.GetDeckEntriesByItemIdAsync(itemId);
            var deckModel = await BuildDeckAsync(entries);

            deckModel.ItemId = deckRecord.ItemId.ToString();
            deckModel.UserId = deckRecord.UserId.ToString();
            deckModel.Name = deckRecord.Name;
            deckModel.Format = deckRecord.Format;
            deckModel.Description = deckRecord.Description;

            return deckModel;
        }

        public async Task<int> CreateDeckAsync(int userId, string name, string format, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Deck name is verplicht.");

            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentException("Format is verplicht.");

            return await _mysqlDeckRepo.CreateDeckAsync(userId, name, format, description);
        }

        public async Task AddCardToDeckAsync(int userId, int itemId, string cardId, int quantity, BoardPart boardPart)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity moet groter zijn dan 0.");

            var deckRecord = await _mysqlDeckRepo.GetDeckRecordByIdAsync(itemId);
            if (deckRecord == null)
                throw new InvalidOperationException("Deck niet gevonden.");

            if (deckRecord.UserId != userId)
                throw new UnauthorizedAccessException("Dit deck hoort niet bij deze gebruiker.");

            var card = await _cardLogicService.GetCardByIdAsync(cardId);
            if (card == null)
                throw new InvalidOperationException("Kaart niet gevonden.");

            if (!card.IsLegalInFormat(deckRecord.Format))
                throw new InvalidOperationException($"Kaart is niet legal in format {deckRecord.Format}.");

            await _mysqlDeckRepo.AddCardToDeckAsync(itemId, cardId, quantity, boardPart);
        }

        public async Task RemoveCardFromDeckAsync(int userId, int itemId, string cardEntryId)
        {
            var deckRecord = await _mysqlDeckRepo.GetDeckRecordByIdAsync(itemId);
            if (deckRecord == null)
                throw new InvalidOperationException("Deck niet gevonden.");

            if (deckRecord.UserId != userId)
                throw new UnauthorizedAccessException("Dit deck hoort niet bij deze gebruiker.");

            await _mysqlDeckRepo.RemoveCardFromDeckAsync(itemId, cardEntryId);
        }

        public async Task<DeckModel> BuildDeckAsync(IEnumerable<DeckCardEntry> entries)
        {
            var deck = new DeckModel();

            foreach (var cardEntry in entries)
            {
                var card = await _cardLogicService.GetCardByIdAsync(cardEntry.CardId);

                if (card == null)
                    continue;

                var deckCard = new DeckCard
                {
                    DeckCardEntryId = cardEntry.Id,
                    Card = card,
                    Quantity = cardEntry.Quantity,
                    BoardPart = (BoardPart)cardEntry.BoardPart // !!!!!  ModelsDL.Database.Models.
                };

                if (cardEntry.BoardPart == BoardPart.Mainboard)
                {
                    deck.Mainboard.Add(deckCard);
                }
                else if (cardEntry.BoardPart == BoardPart.Sideboard)
                {
                    deck.Sideboard.Add(deckCard);
                }
            }

            return deck;
        }

        public async Task RenameDeckAsync(int userId, int itemId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Deck naam is verplicht.");

            var deckRecord = await _mysqlDeckRepo.GetDeckRecordByIdAsync(itemId);
            if (deckRecord == null)
                throw new InvalidOperationException("Deck niet gevonden.");

            if (deckRecord.UserId != userId)
                throw new UnauthorizedAccessException("Dit deck hoort niet bij deze gebruiker.");

            await _mysqlDeckRepo.UpdateDeckNameAsync(itemId, newName);
        }

        public async Task DeleteDeckAsync(int userId, int itemId)
        {
            var deckRecord = await _mysqlDeckRepo.GetDeckRecordByIdAsync(itemId);
            if (deckRecord == null)
                throw new InvalidOperationException("Deck niet gevonden.");

            if (deckRecord.UserId != userId)
                throw new UnauthorizedAccessException("Dit deck hoort niet bij deze gebruiker.");

            await _mysqlDeckRepo.DeleteDeckAsync(itemId);
        }

        public async Task<DeckLegalityResult> ValidateDeckAsync(int itemId)
        {
            var deck = await GetDeckByIdAsync(itemId);

            if (deck == null)
            {
                return new DeckLegalityResult
                {
                    IsLegal = false,
                    Errors = new List<string> { "Deck not found." }
                };
            }

            return _legalityRule.Validate(deck);
        }
    }
}

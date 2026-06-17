using ModelsDL.Database.Models;
using WebAppMTGDAL.Database.Repos;
using WebAppMTGLogic.API.Models;
using WebAppMTGLogic.API.Services;
using WebAppMTGLogic.Database.Models;
using WebAppMTGLogic.Database.Services;
using WebAppMTGLogic.FormatRules.Formats;

namespace ScryfallAPI.test
{
    public class UserDeckServiceTest
    {
        private readonly FakeMySQLDeckRepo _repoFake;
        private readonly FakeCardLogicService _cardLogicFake;
        private readonly FakeLegalityRule _legalityRuleFake;
        private readonly UserDeckService _service;

        public UserDeckServiceTest()
        {
            _repoFake = new FakeMySQLDeckRepo();
            _cardLogicFake = new FakeCardLogicService();
            _legalityRuleFake = new FakeLegalityRule();

            _service = new UserDeckService(
                _repoFake,
                _cardLogicFake,
                _legalityRuleFake);
        }

        [Fact]
        public async Task CreateDeckAsync_ReturnsDeckId_WhenInputIsValid()
        {
            // Arrange
            _repoFake.CreateDeckResult = 32;

            // Act
            var result = await _service.CreateDeckAsync(1, "My Deck", "Standard", "desc");

            // Assert
            Assert.Equal(32, result);
        }

        [Fact]
        public async Task CreateDeckAsync_ThrowsArgumentException_WhenNameIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateDeckAsync(1, "", "Standard", "desc"));
        }

        [Fact]
        public async Task CreateDeckAsync_ThrowsArgumentException_WhenFormatIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateDeckAsync(1, "Deck", "", "desc"));
        }

        [Fact]
        public async Task GetDeckByIdAsync_ReturnsDeckModel_WhenDeckExists()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 2,
                Name = "Test Deck",
                Format = "Standard",
                Description = "desc"
            };

            var entries = new List<DeckCardEntry>();

            _repoFake.DeckRecords.Add(deckRecord);
            _repoFake.DeckEntriesByItemId[1] = entries;

            var result = await _service.GetDeckByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("1", result.ItemId);
            Assert.Equal("2", result.UserId);
            Assert.Equal("Test Deck", result.Name);
            Assert.Equal("Standard", result.Format);
        }

        [Fact]
        public async Task GetDeckByIdAsync_ReturnsNull_WhenDeckDoesNotExist()
        {
            _repoFake.DeckRecords.Clear();

            var result = await _service.GetDeckByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddCardToDeckAsync_CallsRepository_WhenInputIsValid()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 10,
                Format = "Standard"
            };

            var card = new CardReturnModel      
            {
                Standard = "legal" 
            };

            _repoFake.DeckRecords.Add(deckRecord);

            _cardLogicFake.CardsById["card-123"] = card;

            await _service.AddCardToDeckAsync(10, 1, "card-123", 2, BoardPart.Mainboard);

            Assert.True(_repoFake.AddCardToDeckWasCalled);
            Assert.Equal(1, _repoFake.AddCardToDeck_ItemId);
            Assert.Equal("card-123", _repoFake.AddCardToDeck_CardId);
            Assert.Equal(2, _repoFake.AddCardToDeck_Quantity);
            Assert.Equal(BoardPart.Mainboard, _repoFake.AddCardToDeck_BoardPart);
        }

        [Fact]
        public async Task AddCardToDeckAsync_ThrowsArgumentException_WhenQuantityIsZero()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddCardToDeckAsync(1, 1, "card-123", 0, BoardPart.Mainboard));
        }

        [Fact]
        public async Task AddCardToDeckAsync_ThrowsInvalidOperationException_WhenDeckNotFound()
        {
            _repoFake.DeckRecords.Clear();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddCardToDeckAsync(1, 1, "card-123", 1, BoardPart.Mainboard));
        }

        [Fact]
        public async Task AddCardToDeckAsync_ThrowsUnauthorizedAccessException_WhenDeckBelongsToAnotherUser()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 99,
                Format = "Standard"
            };

            _repoFake.DeckRecords.Add(deckRecord);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.AddCardToDeckAsync(1, 1, "card-123", 1, BoardPart.Mainboard));
        }

        [Fact]
        public async Task AddCardToDeckAsync_ThrowsInvalidOperationException_WhenCardNotFound()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 1,
                Format = "Standard"
            };

            _repoFake.DeckRecords.Add(deckRecord);

            _cardLogicFake.CardsById["card-123"] = null;

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddCardToDeckAsync(1, 1, "card-123", 1, BoardPart.Mainboard));
        }

        [Fact]
        public async Task AddCardToDeckAsync_ThrowsInvalidOperationException_WhenCardIsNotLegal()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 1,
                Format = "Standard"
            };

            var card = new CardReturnModel();
            card. Standard = "not_legal" ;

            _repoFake.DeckRecords.Add(deckRecord);
            _cardLogicFake.CardsById["card-123"] = card;

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddCardToDeckAsync(1, 1, "card-123", 1, BoardPart.Mainboard));
        }

        [Fact]
        public async Task RemoveCardFromDeckAsync_CallsRepository_WhenUserOwnsDeck()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 5
            };

            _repoFake.DeckRecords.Add(deckRecord);

            await _service.RemoveCardFromDeckAsync(5, 1, "entry-123");

            Assert.True(_repoFake.RemoveCardFromDeckWasCalled);


        }

        [Fact]
        public async Task RemoveCardFromDeckAsync_ThrowsInvalidOperationException_WhenDeckNotFound()
        {
            _repoFake.DeckRecords.Clear();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.RemoveCardFromDeckAsync(1, 1, "entry-123"));
        }

        [Fact]
        public async Task RemoveCardFromDeckAsync_ThrowsUnauthorizedAccessException_WhenDeckBelongsToAnotherUser()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 99
            };

            _repoFake.DeckRecords.Add(deckRecord);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.RemoveCardFromDeckAsync(1, 1, "entry-123"));
        }

        [Fact]
        public async Task BuildDeckAsync_AddsCardsToCorrectBoardParts()
        {
            var entries = new List<DeckCardEntry>
        {
        new DeckCardEntry
        {
            Id = 1,
            CardId = "card-main",
            Quantity = 2,
            BoardPart = BoardPart.Mainboard
        },
        new DeckCardEntry
        {
            Id = 2,
            CardId = "card-side",
            Quantity = 1,
            BoardPart = BoardPart.Sideboard
        }
        };

            _cardLogicFake.CardsById["card-main"] = new CardReturnModel { Name = "Main Card" };
            _cardLogicFake.CardsById["card-side"] = new CardReturnModel { Name = "Side Card" };

            var result = await _service.BuildDeckAsync(entries);

            Assert.Single(result.Mainboard);
            Assert.Single(result.Sideboard);
            Assert.Equal(2, result.Mainboard[0].Quantity);
            Assert.Equal(1, result.Sideboard[0].Quantity);
        }

        [Fact]
        public async Task BuildDeckAsync_SkipsCard_WhenCardLookupReturnsNull()
        {
            var entries = new List<DeckCardEntry>
        {
        new DeckCardEntry
        {
            Id = 1,
            CardId = "missing-card",
            Quantity = 1,
            BoardPart = BoardPart.Mainboard
        }
        };

            _cardLogicFake.CardsById["missing-card"] = null;

            var result = await _service.BuildDeckAsync(entries);

            Assert.Empty(result.Mainboard);
            Assert.Empty(result.Sideboard);
        }

        [Fact]
        public async Task ValidateDeckAsync_ReturnsIllegalResult_WhenDeckNotFound()
        {
            _repoFake.DeckRecords.Clear();

            var result = await _service.ValidateDeckAsync(1);

            Assert.False(result.IsLegal);
            Assert.Contains("Deck not found.", result.Errors);
        }

        [Fact]
        public async Task ValidateDeckAsync_ReturnsResultFromLegalityRule_WhenDeckExists()
        {
            var deckRecord = new DeckRecord
            {
                ItemId = 1,
                UserId = 1,
                Name = "Deck",
                Format = "Standard",
                Description = "desc"
            };

            var expectedResult = new DeckLegalityResult
            { 
                IsLegal = true, Errors = new List<string>() 
            };

            _repoFake.DeckRecords.Add(deckRecord);

            _repoFake.DeckEntriesByItemId[1] = new List<DeckCardEntry>
            {
                new DeckCardEntry
                {
                    Id = 1,
                    CardId = "card-main",
                    Quantity = 2,
                    BoardPart = BoardPart.Mainboard
                }
            };

            _legalityRuleFake.ResultToReturn = expectedResult;

            var result = await _service.ValidateDeckAsync(1);

            Assert.True(result.IsLegal);
        }
    }
}

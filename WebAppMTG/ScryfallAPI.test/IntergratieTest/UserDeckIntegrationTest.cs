using ModelsDL.Database.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnitTests.test.IntergratieTest.FakeServices;
using WebAppMTGDAL.ScryfallAPI.Services;
using WebAppMTGLogic.API.Services;
using WebAppMTGLogic.Database.Services;
using WebAppMTGLogic.FormatRules.Formats;

namespace UnitTests.test.IntergratieTest
{
    public class UserDeckIntegrationTest
    {
        [Fact]
        public async Task GetDeckByIdAsync_Should_BuildDeckWithMappedCards()
        {
            var repo = new InMemoryDeckRepo();

            repo.Decks.Add(new DeckRecord
            {
                ItemId = 1,
                UserId = 10,
                Name = "Burn",
                Format = "modern",
                Description = "Test deck"
            });

            repo.Entries.Add(new DeckCardEntry
            {
                Id = 1,
                ItemId = 1,
                CardId = "bolt-id",
                Quantity = 4,
                BoardPart = BoardPart.Mainboard
            });

            var handler = new FakeHttpMessageHandler(req =>
            {
                if (req.RequestUri!.ToString() == "https://api.scryfall.com/cards/bolt-id")
                {
                    var json = """
                {
                  "id": "bolt-id",
                  "name": "Lightning Bolt",
                  "mana_cost": "{R}",
                  "oracle_text": "Lightning Bolt deals 3 damage to any target.",
                  "type_line": "Instant",
                  "image_uris": {
                    "normal": "https://img.test/bolt.jpg"
                  },
                  "legalities": {
                    "standard": "not_legal",
                    "modern": "legal",
                    "commander": "legal"
                  }
                }
                """;

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                }

                return new HttpResponseMessage(HttpStatusCode.NotFound);
            });

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.scryfall.com/")
            };

            var scryfallService = new ScryfallService(httpClient);
            var cardLogicService = new CardLogicService(scryfallService);
            var legalityRule = new Standard(); // of andere rule passend bij test
            var userDeckService = new UserDeckService(repo, cardLogicService, legalityRule);

            var deck = await userDeckService.GetDeckByIdAsync(1);

            Assert.NotNull(deck);
            Assert.Equal("Burn", deck!.Name);
            Assert.Single(deck.Mainboard);
            Assert.Equal("Lightning Bolt", deck.Mainboard[0].Card.Name);
            Assert.Equal(4, deck.Mainboard[0].Quantity);
        }

        [Fact]
        public async Task AddCardToDeckAsync_Should_AddCard_WhenCardIsLegal()
        {
            var repo = new InMemoryDeckRepo();

            repo.Decks.Add(new DeckRecord
            {
                ItemId = 1,
                UserId = 10,
                Name = "Control",
                Format = "modern",
                Description = "Test"
            });

            var handler = new FakeHttpMessageHandler(req =>
            {
                var json = """
                {
                    "id": "opt-id",
                    "name": "Opt",
                    "mana_cost": "{U}",
                    "oracle_text": "Scry 1. Draw a card.",
                    "type_line": "Instant",
                "legalities": {
                        "modern": "legal",
                        "standard": "not_legal"
                }
            }
            """;

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.scryfall.com/")
            };

            var scryfallService = new ScryfallService(httpClient);
            var cardLogicService = new CardLogicService(scryfallService);
            var legalityRule = new Standard();
            var service = new UserDeckService(repo, cardLogicService, legalityRule);

            await service.AddCardToDeckAsync(10, 1, "opt-id", 2, BoardPart.Mainboard);

            Assert.Single(repo.Entries);
            Assert.Equal("opt-id", repo.Entries[0].CardId);
            Assert.Equal(2, repo.Entries[0].Quantity);
        }

        [Fact]
        public async Task ValidateDeckAsync_Should_ReturnInvalid_WhenDeckHasTooFewCards()
        {
            var repo = new InMemoryDeckRepo();

            repo.Decks.Add(new DeckRecord
            {
                ItemId = 1,
                UserId = 10,
                Name = "Standard Deck",
                Format = "standard",
                Description = "Test"
            });

            repo.Entries.Add(new DeckCardEntry
            {
                Id = 1,
                ItemId = 1,
                CardId = "opt-id",
                Quantity = 4,
                BoardPart = BoardPart.Mainboard
            });

            var handler = new FakeHttpMessageHandler(req =>
            {
                var json = """
                {
                    "id": "opt-id",
                    "name": "Opt",
                    "mana_cost": "{U}",
                    "oracle_text": "Scry 1. Draw a card.",
                     "type_line": "Instant",
                    "legalities": {
                     "standard": "legal"
                }
            }
            """;

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.scryfall.com/")
            };

            var scryfallService = new ScryfallService(httpClient);
            var cardLogicService = new CardLogicService(scryfallService);
            var legalityRule = new Standard();
            var service = new UserDeckService(repo, cardLogicService, legalityRule);

            var result = await service.ValidateDeckAsync(1);

            Assert.False(result.IsLegal);
            Assert.Contains(result.Errors, e => e.Contains("at least 60"));
        }
    }
}

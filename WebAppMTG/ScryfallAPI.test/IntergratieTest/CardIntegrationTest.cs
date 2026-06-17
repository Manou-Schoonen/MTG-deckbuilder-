using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnitTests.test.IntergratieTest.FakeServices;
using WebAppMTGDAL.ScryfallAPI.Services;
using WebAppMTGLogic.API.Services;

namespace UnitTests.test.IntergratieTest
{
    public class CardIntegrationTest
    {
        [Fact]
        public async Task GetCardByIdAsync_Should_ReturnMappedCard()
        {
            var handler = new FakeHttpMessageHandler(req =>
            {
                Assert.Equal("https://api.scryfall.com/cards/test-card", req.RequestUri!.ToString());

                var json = """
            {
              "id": "test-card",
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
            });

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.scryfall.com/")
            };

            var scryfallService = new ScryfallService(httpClient);
            var cardLogicService = new CardLogicService(scryfallService);

            var result = await cardLogicService.GetCardByIdAsync("test-card");

            Assert.NotNull(result);
            Assert.Equal("test-card", result!.Id);
            Assert.Equal("Lightning Bolt", result.Name);
            Assert.Equal("{R}", result.ManaCost);
            Assert.Equal("legal", result.Modern);
            Assert.Equal("not_legal", result.Standard);
        }
    }
}

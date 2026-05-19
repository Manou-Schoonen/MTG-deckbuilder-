using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebAppMTGDAL.Services;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Services
{
    public class CardLogicService : ICardLogicService
    {
        private readonly IScryfallService _scryfallService;
        public CardLogicService(IScryfallService scryfallService)
        {
            _scryfallService = scryfallService;
        }

        public async Task<List<CardReturnModel>> SearchCardsAsync(string standardSearch)
        {
            if (string.IsNullOrWhiteSpace(standardSearch))
            {
                return new List<CardReturnModel>();
            }

            var cards = await _scryfallService.SearchCardsAsync(standardSearch);

            return MapCards(cards);
        }

        public async Task<List<CardReturnModel>> AdvancedSearchAsync(AdvancedSearchModel search)
        {
            var query = new QueryBuilderAdvanced().Build(search);

            if (string.IsNullOrWhiteSpace(query))
                return new List<CardReturnModel>();

            var cards = await _scryfallService.SearchCardsAsync(query);

            return MapCards(cards);
        }

        public async Task<CardReturnModel?> GetCardByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var card = await _scryfallService.GetCardByIdAsync(id);

            if (card == null)
                return null;

            return MapCard(card);
        }

        private List<CardReturnModel> MapCards(List<ScryfallCardData> cards)
        {
            return cards.Select(MapCard).ToList();
        }

        private CardReturnModel MapCard(ScryfallCardData c)
        {
            return new CardReturnModel
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUris?.Normal,
                ManaCost = c.ManaCost,
                OracleText = c.OracleText,
                TypeLine = c.TypeLine,
                Standard = c.FormatLegality?.Standard,
                Alchemy = c.FormatLegality?.Alchemy,
                Pioneer = c.FormatLegality?.Pioneer,
                Historic = c.FormatLegality?.Historic,
                Modern = c.FormatLegality?.Modern,
                Brawl = c.FormatLegality?.Brawl,
                Legacy = c.FormatLegality?.Legacy,
                Timeless = c.FormatLegality?.Timeless,
                Vintage = c.FormatLegality?.Vintage,
                Pauper = c.FormatLegality?.Pauper,
                Commander = c.FormatLegality?.Commander,
                Penny = c.FormatLegality?.Penny,
                Oathbreaker = c.FormatLegality?.Oathbreaker,
                Gladiator = c.FormatLegality?.Gladiator
            };
        }
    }
}
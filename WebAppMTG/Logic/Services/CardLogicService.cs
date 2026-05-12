using System;
using DAL.Models;
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
            var query = new QueryBuilder().Build(search);

            if (string.IsNullOrWhiteSpace(query))
                return new List<CardReturnModel>();

            var cards = await _scryfallService.SearchCardsAsync(query);

            return MapCards(cards);
        }

        private List<CardReturnModel> MapCards(List<ScryfallCardData> cards)
        {
            return cards.Select(c => new CardReturnModel
            {
                Name = c.Name,
                ImageUrl = c.ImageUris?.Normal,
                ManaCost = c.ManaCost,
                OracleText = c.OracleText,
                Rarity = c.Rarity,
                TypeLine = c.TypeLine
            }).ToList();
        }
    }
}
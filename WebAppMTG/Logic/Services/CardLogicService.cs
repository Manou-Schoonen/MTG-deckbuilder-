using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
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

        public async Task<List<CardModel>> SearchCardsAsync(string standardSearch)
        {
            if (string.IsNullOrWhiteSpace(standardSearch))
            {
                return new List<CardModel>();
            }

            var cards = await _scryfallService.SearchCardsAsync(standardSearch);

            return cards.Select(c => new CardModel
            {
                Name = c.Name,
                ImageUrl = c.ImageUris?.Normal,
                ManaCost = c.ManaCost
            }).ToList();
        }
    }
}

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

        public async Task<List<CardModel>> SearchCardsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new List<CardModel>();
            }

            var cards = await _scryfallService.SearchCardsAsync(searchText);

            return cards.Select(c => new CardModel
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUris?.Normal
            }).ToList();
        }
    }
}

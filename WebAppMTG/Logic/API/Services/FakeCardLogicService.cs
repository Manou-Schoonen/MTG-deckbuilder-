using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.API.Services
{
    public class FakeCardLogicService : ICardLogicService
    {
        public Dictionary<string, CardReturnModel?> CardsById { get; set; } = new();

        public List<CardReturnModel> SearchCardsResult { get; set; } = new();
        public List<CardReturnModel> AdvancedSearchResult { get; set; } = new();

        public Task<List<CardReturnModel>> SearchCardsAsync(string standardSearch)
        {
            return Task.FromResult(SearchCardsResult);
        }

        public Task<List<CardReturnModel>> AdvancedSearchAsync(AdvancedSearchModel search)
        {
            return Task.FromResult(AdvancedSearchResult);
        }

        public Task<CardReturnModel?> GetCardByIdAsync(string id)
        {
            CardsById.TryGetValue(id, out var card);
            return Task.FromResult(card);
        }
    }
}

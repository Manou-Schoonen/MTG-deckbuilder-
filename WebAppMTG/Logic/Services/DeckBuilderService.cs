using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.Interfaces;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Services
{
    public class DeckBuilderService : IDeckBuilderService
    {
        private readonly ICardLogicService _cardLogicService;

        public DeckBuilderService(ICardLogicService cardLogicService)
        {
            _cardLogicService = cardLogicService;
        }

        public async Task<DeckModel> BuildDeckAsync(IEnumerable<CardDeckEntry> entries)
        {
            var deck = new DeckModel();

            foreach (var entry in entries)
            {
                var card = await _cardLogicService.GetCardByIdAsync(entry.CardId);

                if (card == null)
                    continue;

                var deckCard = new DeckCard
                {
                    Card = card,
                    Quantity = entry.Quantity,
                    BoardPart = entry.BoardPart
                };

                if (entry.BoardPart == BoardPart.Mainboard)
                {
                    deck.Mainboard.Add(deckCard);
                }
                else if (entry.BoardPart == BoardPart.Sideboard)
                {
                    deck.Sideboard.Add(deckCard);
                }
            }

            return deck;
        }
    }
}

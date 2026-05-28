using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Interfaces;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.FormatRules
{
    public class Standard : ILegalityRule
    {
        public string FormatName => "standard";

        public DeckLegalityResult Validate(DeckModel deck)
        {
            var result = new DeckLegalityResult();

            var mainboardCount = deck.Mainboard.Sum(c => c.Quantity);

            if (mainboardCount < 60)
            {
                result.Errors.Add("A Standard deck must contain at least 60 cards in the mainboard.");
            }

            if (deck.Sideboard.Sum(c => c.Quantity) > 15)
            {
                result.Errors.Add("A Standard sideboard may contain at most 15 cards.");
            }

            ValidateMainboard(deck, result);

            ValidateSideboard(deck, result);

            result.IsLegal = result.Errors.Count == 0;
            return result;
        }

        private static void ValidateMainboard(DeckModel deck, DeckLegalityResult result)
        {
            foreach (var deckCard in deck.Mainboard)
            {
                var card = deckCard.Card;

                if (!card.IsLegalInFormat("standard"))
                {
                    result.Errors.Add($"{card.Name} is not legal in Standard.");
                }

                if (!card.IsBasicLand && deckCard.Quantity > 4)
                {
                    result.Errors.Add($"{card.Name} has more than 4 copies.");
                }
            }
        }

        private static void ValidateSideboard(DeckModel deck, DeckLegalityResult result)
        {
            foreach (var deckCard in deck.Sideboard)
            {
                var card = deckCard.Card;

                if (!card.IsLegalInFormat("standard"))
                {
                    result.Errors.Add($"{card.Name} is not legal in Standard.");
                }

                if (!card.IsBasicLand && deckCard.Quantity > 4)
                {
                    result.Errors.Add($"{card.Name} has more than 4 copies in the sideboard.");
                }
            }
        }
    }
}

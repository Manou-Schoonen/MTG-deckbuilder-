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
        public int MainBoardCardLimit => 60;
        public int SideBoardCardLimit => 15;
        public int UniqueCardLimit => 4;

        public DeckLegalityResult Validate(DeckModel deck)
        {
            var result = new DeckLegalityResult();

            var mainboardCount = deck.Mainboard.Sum(c => c.Quantity);

            if (mainboardCount < MainBoardCardLimit)
            {
                result.Errors.Add($"A Standard deck must contain at least {MainBoardCardLimit} cards in the mainboard.");
            }

            if (deck.Sideboard.Sum(c => c.Quantity) > SideBoardCardLimit)
            {
                result.Errors.Add($"A Standard sideboard may contain at most {SideBoardCardLimit} cards.");
            }

            ValidateMainboard(deck, result);

            ValidateSideboard(deck, result);

            result.IsLegal = result.Errors.Count == 0;
            return result;
        }

        private void ValidateMainboard(DeckModel deck, DeckLegalityResult result)
        {
            foreach (var deckCard in deck.Mainboard)
            {
                var card = deckCard.Card;

                if (!card.IsLegalInFormat("standard"))
                {
                    result.Errors.Add($"{card.Name} is not legal in Standard.");
                }

                if (!card.IsBasicLand && deckCard.Quantity > UniqueCardLimit)
                {
                    result.Errors.Add($"{card.Name} has more than {UniqueCardLimit} copies.");
                }
            }
        }

        private void ValidateSideboard(DeckModel deck, DeckLegalityResult result)
        {
            foreach (var deckCard in deck.Sideboard)
            {
                var card = deckCard.Card;

                if (!card.IsLegalInFormat("standard"))
                {
                    result.Errors.Add($"{card.Name} is not legal in Standard.");
                }

                if (!card.IsBasicLand && deckCard.Quantity > UniqueCardLimit)
                {
                    result.Errors.Add($"{card.Name} has more than {UniqueCardLimit} copies in the sideboard.");
                }
            }
        }
    }
}

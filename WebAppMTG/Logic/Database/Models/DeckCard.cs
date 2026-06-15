using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.Database.Models
{
    public class DeckCard
    {
        public int DeckCardEntryId { get; set; }
        public CardReturnModel Card { get; set; } = new();
        public int Quantity { get; set; }
        public ModelsDL.Database.Models.BoardPart BoardPart { get; set; }
    }
}

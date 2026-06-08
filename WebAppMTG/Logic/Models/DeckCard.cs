using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.Models
{
    public class DeckCard
    {
        public int DeckCardEntryId { get; set; }
        public CardReturnModel Card { get; set; } = new();
        public int Quantity { get; set; }
        public BoardPart BoardPart { get; set; }
    }
    public enum BoardPart
    {
        Mainboard,
        Sideboard
    }
}

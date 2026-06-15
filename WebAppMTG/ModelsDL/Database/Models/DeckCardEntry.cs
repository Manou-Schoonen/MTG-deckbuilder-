using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsDL.Database.Models
{
    public class DeckCardEntry
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string CardId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public BoardPart BoardPart { get; set; }
    }
    public enum BoardPart
    {
        Mainboard,
        Sideboard
    }
}
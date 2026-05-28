using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppMTGLogic.Models
{
    public class CardDeckEntry
    {
        public string CardId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public BoardPart BoardPart { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAppMTGLogic.Models
{
    public class CardModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ManaCost { get; set; }
        public string? TypeLine { get; set; }
        public string? OracleText { get; set; }
        public string? Set { get; set; }
        public string? Rarity { get; set; }
        public string? ImageUrl { get; set; }
    }
}

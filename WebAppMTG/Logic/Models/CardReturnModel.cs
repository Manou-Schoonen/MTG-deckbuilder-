using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAppMTGLogic.Models
{
    public class CardReturnModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ManaCost { get; set; }
        public string? TypeLine { get; set; }
        public string? OracleText { get; set; }
        public string? ImageUrl { get; set; }

        public string? Standard { get; set; }
        public string? Historic { get; set; }
        public string? Gladiator { get; set; }
        public string? Pioneer { get; set; }
        public string? Modern { get; set; }
        public string? Legacy { get; set; }
        public string? Pauper { get; set; }
        public string? Vintage { get; set; }
        public string? Penny { get; set; }
        public string? Commander { get; set; }
        public string? Brawl { get; set; }
        public string? Alchemy { get; set; }
        public string? Oathbreaker { get; set; }
        public string? Timeless { get; set; }
    }
}

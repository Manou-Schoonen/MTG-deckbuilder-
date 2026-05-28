using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAppMTGLogic.API.Models
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
        public bool IsBasicLand =>
        TypeLine?.Contains("Basic Land", StringComparison.OrdinalIgnoreCase) == true;

        public bool IsLegalInFormat(string format)
        {
            return format.ToLower() switch
            {
                "standard" => string.Equals(Standard, "legal", StringComparison.OrdinalIgnoreCase),
                "alchemy" => string.Equals(Alchemy, "legal", StringComparison.OrdinalIgnoreCase),
                "pioneer" => string.Equals(Pioneer, "legal", StringComparison.OrdinalIgnoreCase),
                "historic" => string.Equals(Historic, "legal", StringComparison.OrdinalIgnoreCase),
                "modern" => string.Equals(Modern, "legal", StringComparison.OrdinalIgnoreCase),
                "brawl" => string.Equals(Brawl, "legal", StringComparison.OrdinalIgnoreCase),
                "legacy" => string.Equals(Legacy, "legal", StringComparison.OrdinalIgnoreCase),
                "timeless" => string.Equals(Timeless, "legal", StringComparison.OrdinalIgnoreCase),
                "vintage" => string.Equals(Vintage, "legal", StringComparison.OrdinalIgnoreCase),
                "pauper" => string.Equals(Pauper, "legal", StringComparison.OrdinalIgnoreCase),
                "commander" => string.Equals(Commander, "legal", StringComparison.OrdinalIgnoreCase),
                "penny" => string.Equals(Penny, "legal", StringComparison.OrdinalIgnoreCase),
                "oathbreaker" => string.Equals(Oathbreaker, "legal", StringComparison.OrdinalIgnoreCase),
                "gladiator" => string.Equals(Gladiator, "legal", StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }
    }
}

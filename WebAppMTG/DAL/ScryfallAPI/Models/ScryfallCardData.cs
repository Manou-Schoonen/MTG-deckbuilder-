using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAppMTGDAL.ScryfallAPI.Models
{
    public class ScryfallCardData
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("mana_cost")]
        public string? ManaCost { get; set; }

        [JsonPropertyName("type_line")]
        public string? TypeLine { get; set; }

        [JsonPropertyName("oracle_text")]
        public string? OracleText { get; set; }

        [JsonPropertyName("set")]
        public string? Set { get; set; }

        [JsonPropertyName("rarity")]
        public string? Rarity { get; set; }

        [JsonPropertyName("image_uris")]
        public ImageUris? ImageUris { get; set; }

        [JsonPropertyName("legalities")]
        public ScryfallFormatLegality? FormatLegality { get; set; }
    }


    public class ImageUris
    {
        [JsonPropertyName("small")]
        public string? Small { get; set; }

        [JsonPropertyName("normal")]
        public string? Normal { get; set; }

        [JsonPropertyName("large")]
        public string? Large { get; set; }
    }

    public class ScryfallFormatLegality
    {
        [JsonPropertyName("standard")]
        public string? Standard { get; set; }

        [JsonPropertyName("historic")]
        public string? Historic { get; set; }

        [JsonPropertyName("gladiator")]
        public string? Gladiator { get; set; }

        [JsonPropertyName("pioneer")]
        public string? Pioneer { get; set; }

        [JsonPropertyName("modern")]
        public string? Modern { get; set; }

        [JsonPropertyName("legacy")]
        public string? Legacy { get; set; }

        [JsonPropertyName("pauper")]
        public string? Pauper { get; set; }

        [JsonPropertyName("vintage")]
        public string? Vintage { get; set; }

        [JsonPropertyName("penny")]
        public string? Penny { get; set; }

        [JsonPropertyName("commander")]
        public string? Commander { get; set; }

        [JsonPropertyName("alchemy")]
        public string? Alchemy { get; set; }

        [JsonPropertyName("brawl")]
        public string? Brawl { get; set; }

        [JsonPropertyName("oathbreaker")]
        public string? Oathbreaker { get; set; }
        
        [JsonPropertyName("timeless")]
        public string? Timeless { get; set; }
    }
}
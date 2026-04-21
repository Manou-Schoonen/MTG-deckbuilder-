using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DAL.Models
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
    }

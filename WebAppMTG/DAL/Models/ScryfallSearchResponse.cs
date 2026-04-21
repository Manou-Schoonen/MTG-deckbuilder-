using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DAL.Models
{
    public class ScryfallSearchResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("next_page")]
        public string? NextPage { get; set; }

        [JsonPropertyName("data")]
        public List<ScryfallCardData> Data { get; set; } = new();
    }
}

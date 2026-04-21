using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace DAL.Services
{
    public class ScryfallService
    {
        private readonly HttpClient _httpClient;

        public ScryfallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ScryfallCardData>> SearchCardsAsync(string standardSearch)
        {
            var url = $"cards/search?q={Uri.EscapeDataString(standardSearch)}";
            var httpResponse = await _httpClient.GetAsync(url);

            //if (!httpResponse.IsSuccessStatusCode)
            //{
                //var errorContent = await httpResponse.Content.ReadAsStringAsync();
                //throw new Exception($"Scryfall error: {(int)httpResponse.StatusCode} - {errorContent}");
            //}
            var response = await httpResponse.Content.ReadFromJsonAsync<ScryfallSearchResponse>();
            return response?.Data ?? new List<ScryfallCardData>();
        }
    }
}

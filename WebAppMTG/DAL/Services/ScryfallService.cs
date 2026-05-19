using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using WebAppMTGDAL.Services;

namespace DAL.Services
{
    public class ScryfallService : IScryfallService
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

            var response = await httpResponse.Content.ReadFromJsonAsync<ScryfallSearchResponse>();
            return response?.Data ?? new List<ScryfallCardData>();
        }
        public async Task<ScryfallCardData?> GetCardByIdAsync(string id)
        {
            var url = $"cards/{id}";
            return await _httpClient.GetFromJsonAsync<ScryfallCardData>(url);
        }
    }
}

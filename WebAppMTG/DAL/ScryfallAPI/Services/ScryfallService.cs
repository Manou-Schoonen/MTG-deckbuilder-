using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.API.Models;
using WebAppMTGModelsDL.Exeptions;

namespace WebAppMTGDAL.ScryfallAPI.Services
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
            try
            {
                var url = $"cards/search?q={Uri.EscapeDataString(standardSearch)}";
                var httpResponse = await _httpClient.GetAsync(url);

                httpResponse.EnsureSuccessStatusCode();

                var response = await httpResponse.Content.ReadFromJsonAsync<ScryfallSearchResponse>();
                return response?.Data ?? new List<ScryfallCardData>();
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalApiUnavailableException("De kaarten-API is momenteel niet beschikbaar.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new ExternalApiUnavailableException("De kaarten-API reageert te langzaam of niet.", ex);
            }
        }

        public async Task<ScryfallCardData?> GetCardByIdAsync(string id)
        {
            try
            {
                var url = $"cards/{id}";
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ScryfallCardData>();
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalApiUnavailableException("De kaarten-API is momenteel niet beschikbaar.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new ExternalApiUnavailableException("De kaarten-API reageert te langzaam of niet.", ex);
            }
        }
    }
}

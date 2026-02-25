using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class CostElementsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public CostElementsService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<CostElements>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/costelements");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CostElements>>();
        }

        // ── Get by ElementID ──
        public async Task<CostElements> GetByIdAsync(long elementId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/costelements/id/{elementId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<CostElements>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(CostElements costElement)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/costelements", costElement);
            return response.IsSuccessStatusCode;
        }
    }
}

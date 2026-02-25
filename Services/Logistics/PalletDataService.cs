using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class PalletDataService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PalletDataService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<PalletData>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletdata");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletData>>();
        }

        // ── Get by PalletID ──
        public async Task<PalletData> GetByIdAsync(string palletId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletdata/id/{Uri.EscapeDataString(palletId)}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<PalletData>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(PalletData palletData)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/palletdata", palletData);
            return response.IsSuccessStatusCode;
        }
    }
}

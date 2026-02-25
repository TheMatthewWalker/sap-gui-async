using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class CostTypesService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public CostTypesService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<CostTypes>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/costtypes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CostTypes>>();
        }

        // ── Get by TypeID ──
        public async Task<CostTypes> GetByIdAsync(long typeId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/costtypes/id/{typeId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<CostTypes>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(CostTypes costType)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/costtypes", costType);
            return response.IsSuccessStatusCode;
        }
    }
}

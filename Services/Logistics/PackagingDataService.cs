using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class PackagingDataService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PackagingDataService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<PackagingData>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/packagingdata");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PackagingData>>();
        }

        // ── Get by PackID ──
        public async Task<PackagingData> GetByIdAsync(string packId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/packagingdata/id/{Uri.EscapeDataString(packId)}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<PackagingData>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(PackagingData packagingData)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/packagingdata", packagingData);
            return response.IsSuccessStatusCode;
        }
    }
}

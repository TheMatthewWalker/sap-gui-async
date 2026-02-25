using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class CostCentersService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public CostCentersService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<CostCenters>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/costcenters");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CostCenters>>();
        }

        // ── Get by CenterID ──
        public async Task<CostCenters> GetByIdAsync(long centerId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/costcenters/id/{centerId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<CostCenters>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(CostCenters costCenter)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/costcenters", costCenter);
            return response.IsSuccessStatusCode;
        }
    }
}

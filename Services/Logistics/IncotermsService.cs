using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class IncotermsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public IncotermsService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<Incoterms>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/incoterms");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Incoterms>>();
        }

        // ── Get by IncotermsID ──
        public async Task<Incoterms> GetByIdAsync(string incotermsId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/incoterms/id/{Uri.EscapeDataString(incotermsId)}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<Incoterms>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(Incoterms incoterms)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/incoterms", incoterms);
            return response.IsSuccessStatusCode;
        }
    }
}

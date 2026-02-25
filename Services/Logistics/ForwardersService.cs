using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class ForwardersService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ForwardersService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<Forwarders>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/forwarders");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Forwarders>>();
        }

        // ── Get by ForwarderID ──
        public async Task<Forwarders> GetByIdAsync(long forwarderId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/forwarders/id/{forwarderId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<Forwarders>>();
            return results.FirstOrDefault();
        }

        // ── Get approved forwarders only ──
        public async Task<List<Forwarders>> GetApprovedAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/forwarders/approved");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Forwarders>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(Forwarders forwarder)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/forwarders", forwarder);
            return response.IsSuccessStatusCode;
        }
    }
}

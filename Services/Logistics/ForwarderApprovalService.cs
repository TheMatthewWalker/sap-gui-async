using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class ForwarderApprovalService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ForwarderApprovalService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<ForwarderApproval>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/forwarderapproval");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ForwarderApproval>>();
        }

        // ── Get by ForwarderID ──
        public async Task<ForwarderApproval> GetByIdAsync(long forwarderId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/forwarderapproval/id/{forwarderId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<ForwarderApproval>>();
            return results.FirstOrDefault();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(ForwarderApproval forwarderApproval)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/forwarderapproval", forwarderApproval);
            return response.IsSuccessStatusCode;
        }
    }
}

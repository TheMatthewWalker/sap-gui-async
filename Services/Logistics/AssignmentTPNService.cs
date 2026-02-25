using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class AssignmentTPNService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public AssignmentTPNService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<AssignmentTPN>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/assignmenttpn");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AssignmentTPN>>();
        }

        // ── Get by PostalZone ──
        public async Task<List<AssignmentTPN>> GetByPostalZoneAsync(string postalZone)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/assignmenttpn/zone/{Uri.EscapeDataString(postalZone)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AssignmentTPN>>();
        }

        // ── Get by PostalCode ──
        public async Task<List<AssignmentTPN>> GetByPostalCodeAsync(string postalCode)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/assignmenttpn/postalcode/{Uri.EscapeDataString(postalCode)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AssignmentTPN>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(AssignmentTPN assignmentTPN)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/assignmenttpn", assignmentTPN);
            return response.IsSuccessStatusCode;
        }
    }
}

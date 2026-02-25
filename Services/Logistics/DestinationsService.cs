using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class DestinationsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public DestinationsService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<Destinations>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/destinations");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Destinations>>();
        }

        // ── Get by DestinationID ──
        public async Task<Destinations> GetByIdAsync(long destinationId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/destinations/id/{destinationId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<Destinations>>();
            return results.FirstOrDefault();
        }

        // ── Get by Country ──
        public async Task<List<Destinations>> GetByCountryAsync(string country)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/destinations/country/{Uri.EscapeDataString(country)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Destinations>>();
        }

        // ── Get by Zone ──
        public async Task<List<Destinations>> GetByZoneAsync(string zone)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/destinations/zone/{Uri.EscapeDataString(zone)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Destinations>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(Destinations destination)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/destinations", destination);
            return response.IsSuccessStatusCode;
        }
    }
}

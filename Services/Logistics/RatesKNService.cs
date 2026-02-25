using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class RatesKNService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public RatesKNService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<RatesKN>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/rateskn");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatesKN>>();
        }

        // ── Get by CountryCode ──
        public async Task<List<RatesKN>> GetByCountryAsync(string countryCode)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/rateskn/country/{Uri.EscapeDataString(countryCode)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatesKN>>();
        }

        // ── Get by PostalCode ──
        public async Task<List<RatesKN>> GetByPostalCodeAsync(string postalCode)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/rateskn/postalcode/{Uri.EscapeDataString(postalCode)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatesKN>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(RatesKN rateKN)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/rateskn", rateKN);
            return response.IsSuccessStatusCode;
        }
    }
}

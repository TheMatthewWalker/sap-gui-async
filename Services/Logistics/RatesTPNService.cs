using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class RatesTPNService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public RatesTPNService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<RatesTPN>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/ratestpn");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatesTPN>>();
        }

        // ── Get by PostalZone ──
        public async Task<List<RatesTPN>> GetByPostalZoneAsync(string postalZone)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/ratestpn/zone/{Uri.EscapeDataString(postalZone)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatesTPN>>();
        }

        // ── Get by PalletCategory ──
        public async Task<List<RatesTPN>> GetByPalletCategoryAsync(string palletCategory)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/ratestpn/category/{Uri.EscapeDataString(palletCategory)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatesTPN>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(RatesTPN rateTPN)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/ratestpn", rateTPN);
            return response.IsSuccessStatusCode;
        }
    }
}

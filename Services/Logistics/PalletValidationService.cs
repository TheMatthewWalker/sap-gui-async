using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class PalletValidationService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PalletValidationService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<PalletValidation>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletvalidation");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletValidation>>();
        }

        // ── Get by PalletID ──
        public async Task<List<PalletValidation>> GetByPalletIdAsync(string palletId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletvalidation/pallet/{Uri.EscapeDataString(palletId)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletValidation>>();
        }

        // ── Get by PackagingID ──
        public async Task<List<PalletValidation>> GetByPackagingIdAsync(string packagingId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletvalidation/packaging/{Uri.EscapeDataString(packagingId)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletValidation>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(PalletValidation palletValidation)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/palletvalidation", palletValidation);
            return response.IsSuccessStatusCode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class PalletMainService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PalletMainService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<PalletMain>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletmain");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletMain>>();
        }

        // ── Get by PalletID ──
        public async Task<PalletMain> GetByIdAsync(long palletId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletmain/id/{palletId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<PalletMain>>();
            return results.FirstOrDefault();
        }

        // ── Get by Category ──
        public async Task<List<PalletMain>> GetByCategoryAsync(string category)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletmain/category/{Uri.EscapeDataString(category)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletMain>>();
        }

        // ── Get by Location ──
        public async Task<List<PalletMain>> GetByLocationAsync(string location)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletmain/location/{Uri.EscapeDataString(location)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletMain>>();
        }

        // ── Create new record ──
        // palletID is an IDENTITY column. The server assigns it and returns it in the response.
        public async Task<long?> CreateAsync(PalletMain palletMain)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/palletmain", palletMain);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResult>();
            return result?.PalletID;
        }

        private class CreateResult
        {
            public long? PalletID { get; set; }
        }
    }
}

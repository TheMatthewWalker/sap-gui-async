using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class PalletPackagesService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PalletPackagesService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<PalletPackages>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletpackages");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletPackages>>();
        }

        // ── Get by PalletItemID ──
        public async Task<PalletPackages> GetByIdAsync(long palletItemId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletpackages/id/{palletItemId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<PalletPackages>>();
            return results.FirstOrDefault();
        }

        // ── Get by PalletID ──
        public async Task<List<PalletPackages>> GetByPalletIdAsync(long palletId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletpackages/pallet/{palletId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletPackages>>();
        }

        // ── Get by SAP Delivery ──
        public async Task<List<PalletPackages>> GetBySapDeliveryAsync(string sapDelivery)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletpackages/sapdelivery/{Uri.EscapeDataString(sapDelivery)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletPackages>>();
        }

        // ── Get by SAP Material ──
        public async Task<List<PalletPackages>> GetBySapMaterialAsync(string sapMaterial)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/palletpackages/sapmaterial/{Uri.EscapeDataString(sapMaterial)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PalletPackages>>();
        }

        // ── Create new record ──
        // palletItemID is an IDENTITY column. The server assigns it and returns it in the response.
        public async Task<long?> CreateAsync(PalletPackages palletPackage)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/palletpackages", palletPackage);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResult>();
            return result?.PalletItemID;
        }

        private class CreateResult
        {
            public long? PalletItemID { get; set; }
        }
    }
}

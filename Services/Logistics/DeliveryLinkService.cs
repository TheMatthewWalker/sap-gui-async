using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class DeliveryLinkService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public DeliveryLinkService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<DeliveryLink>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverylink");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryLink>>();
        }

        // ── Get by DeliveryID ──
        public async Task<List<DeliveryLink>> GetByDeliveryIdAsync(long deliveryId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverylink/delivery/{deliveryId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryLink>>();
        }

        // ── Get by PalletID ──
        public async Task<List<DeliveryLink>> GetByPalletIdAsync(long palletId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverylink/pallet/{palletId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryLink>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(DeliveryLink deliveryLink)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/deliverylink", deliveryLink);
            return response.IsSuccessStatusCode;
        }
    }
}

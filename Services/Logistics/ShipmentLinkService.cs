using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class ShipmentLinkService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ShipmentLinkService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<ShipmentLink>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentlink");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentLink>>();
        }

        // ── Get by ShipmentID ──
        public async Task<List<ShipmentLink>> GetByShipmentIdAsync(long shipmentId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentlink/shipment/{shipmentId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentLink>>();
        }

        // ── Get by DeliveryID ──
        public async Task<List<ShipmentLink>> GetByDeliveryIdAsync(long deliveryId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentlink/delivery/{deliveryId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentLink>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(ShipmentLink shipmentLink)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/shipmentlink", shipmentLink);
            return response.IsSuccessStatusCode;
        }
    }
}

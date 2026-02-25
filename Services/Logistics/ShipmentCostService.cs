using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class ShipmentCostService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ShipmentCostService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<ShipmentCost>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentcost");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentCost>>();
        }

        // ── Get by CostID ──
        public async Task<ShipmentCost> GetByIdAsync(long costId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentcost/id/{costId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<ShipmentCost>>();
            return results.FirstOrDefault();
        }

        // ── Get by ShipmentID ──
        public async Task<List<ShipmentCost>> GetByShipmentIdAsync(long shipmentId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentcost/shipment/{shipmentId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentCost>>();
        }

        // ── Get by CostType ──
        public async Task<List<ShipmentCost>> GetByCostTypeAsync(string costType)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentcost/costtype/{Uri.EscapeDataString(costType)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentCost>>();
        }

        // ── Create new record ──
        // costID is an IDENTITY column. The server assigns it and returns it in the response.
        public async Task<long?> CreateAsync(ShipmentCost shipmentCost)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/shipmentcost", shipmentCost);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResult>();
            return result?.CostID;
        }

        private class CreateResult
        {
            public long? CostID { get; set; }
        }
    }
}

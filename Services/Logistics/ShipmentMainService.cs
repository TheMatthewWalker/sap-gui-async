using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class ShipmentMainService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ShipmentMainService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<ShipmentMain>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentmain");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentMain>>();
        }

        // ── Get by ShipmentID ──
        public async Task<ShipmentMain> GetByIdAsync(long shipmentId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentmain/id/{shipmentId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<ShipmentMain>>();
            return results.FirstOrDefault();
        }

        // ── Get by ForwarderID ──
        public async Task<List<ShipmentMain>> GetByForwarderAsync(long forwarderId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentmain/forwarder/{forwarderId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentMain>>();
        }

        // ── Get by DestinationID ──
        public async Task<List<ShipmentMain>> GetByDestinationAsync(long destinationId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/shipmentmain/destination/{destinationId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentMain>>();
        }

        // ── Get by planned collection date range ──
        public async Task<List<ShipmentMain>> GetByPlannedCollectionRangeAsync(string dateFrom, string dateTo)
        {
            var url = $"{_baseUrl}/api/shipmentmain/daterange?dateFrom={Uri.EscapeDataString(dateFrom)}&dateTo={Uri.EscapeDataString(dateTo)}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ShipmentMain>>();
        }

        // ── Create new record ──
        // shipmentID is an IDENTITY column. The server assigns it and returns it in the response.
        public async Task<long?> CreateAsync(ShipmentMain shipmentMain)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/shipmentmain", shipmentMain);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResult>();
            return result?.ShipmentID;
        }

        private class CreateResult
        {
            public long? ShipmentID { get; set; }
        }
    }
}

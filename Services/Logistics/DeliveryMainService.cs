using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class DeliveryMainService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public DeliveryMainService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<DeliveryMain>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverymain");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryMain>>();
        }

        // ── Get by DeliveryID ──
        public async Task<DeliveryMain> GetByIdAsync(long deliveryId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverymain/id/{deliveryId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<DeliveryMain>>();
            return results.FirstOrDefault();
        }

        // ── Get by CustomerID ──
        public async Task<List<DeliveryMain>> GetByCustomerAsync(long customerId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverymain/customer/{customerId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryMain>>();
        }

        // ── Get by Operator ──
        public async Task<List<DeliveryMain>> GetByOperatorAsync(string operatorName)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/deliverymain/operator/{Uri.EscapeDataString(operatorName)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryMain>>();
        }

        // ── Get by due date range ──
        public async Task<List<DeliveryMain>> GetByDueDateRangeAsync(string dateFrom, string dateTo)
        {
            var url = $"{_baseUrl}/api/deliverymain/daterange?dateFrom={Uri.EscapeDataString(dateFrom)}&dateTo={Uri.EscapeDataString(dateTo)}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DeliveryMain>>();
        }

        // ── Create new record ──
        public async Task<bool> CreateAsync(DeliveryMain deliveryMain)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/deliverymain", deliveryMain);
            return response.IsSuccessStatusCode;
        }
    }
}

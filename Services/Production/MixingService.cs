using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using costing_tool.Models;

namespace costing_tool.Services
{
    public class MixingService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public MixingService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        // ── Get all records ──
        public async Task<List<Mixing>> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/mixing");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }

        // ── Get by MixingID ──
        public async Task<Mixing> GetByIdAsync(string mixingId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/mixing/id/{mixingId}");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<List<Mixing>>();
            return results.FirstOrDefault();
        }

        // ── Get by Operator ──
        public async Task<List<Mixing>> GetByOperatorAsync(string operatorName)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/mixing/operator/{Uri.EscapeDataString(operatorName)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }

        // ── Get by Shift ──
        public async Task<List<Mixing>> GetByShiftAsync(string shift)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/mixing/shift/{Uri.EscapeDataString(shift)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }

        // ── Get by MixCode ──
        public async Task<List<Mixing>> GetByMixCodeAsync(string mixCode)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/mixing/mixcode/{Uri.EscapeDataString(mixCode)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }

        // ── Get by SupplierBatch ──
        public async Task<List<Mixing>> GetBySupplierBatchAsync(string supplierBatch)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/mixing/supplierbatch/{Uri.EscapeDataString(supplierBatch)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }

        // ── Get by date range ──
        public async Task<List<Mixing>> GetByDateRangeAsync(string dateFrom, string dateTo)
        {
            var url = $"{_baseUrl}/api/mixing/daterange?dateFrom={Uri.EscapeDataString(dateFrom)}&dateTo={Uri.EscapeDataString(dateTo)}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }



        // ── Dynamic filtered search ──
        public async Task<List<Mixing>> SearchAsync(IEnumerable<MixingCriteria> searchRows)
        {
            var activeRows = searchRows
                .Where(r => !string.IsNullOrWhiteSpace(r.MixingID) ||
                            !string.IsNullOrWhiteSpace(r.MixCode) ||
                            !string.IsNullOrWhiteSpace(r.TotalWeight) ||
                            !string.IsNullOrWhiteSpace(r.Shift) ||
                            !string.IsNullOrWhiteSpace(r.Operator) ||
                            !string.IsNullOrWhiteSpace(r.SupplierBatch) ||
                            !string.IsNullOrWhiteSpace(r.BatchTub) ||
                            r.CreationDate.HasValue ||
                            r.DateTo.HasValue ||
                            !string.IsNullOrWhiteSpace(r.CreationTime) ||
                            !string.IsNullOrWhiteSpace(r.Comment))
                .Select(r => new MixingSearchDto
                {
                    mixingID = r.MixingID,
                    mixCode = r.MixCode,
                    totalWeight = r.TotalWeight,
                    shift = r.Shift,
                    @operator = r.Operator,
                    supplierBatch = r.SupplierBatch,
                    batchTub = r.BatchTub,
                    creationDate = r.CreationDateFormatted,  // Converts DateTimeOffset → dd.MM.yyyy
                    dateTo = r.DateToFormatted,        // Converts DateTimeOffset → dd.MM.yyyy
                    creationTime = r.CreationTime,
                    comment = r.Comment
                })
                .ToList();

            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/mixing/search", activeRows);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Mixing>>();
        }


        // ── Create new record ──
        public async Task<bool> CreateAsync(Mixing mixing)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/mixing", mixing);
            return response.IsSuccessStatusCode;
        }

        // ── Update record ──
        public async Task<bool> UpdateAsync(string mixingId, Mixing mixing)
        {
            var response = await _client.PutAsJsonAsync($"{_baseUrl}/api/mixing/{mixingId}", mixing);
            return response.IsSuccessStatusCode;
        }
    }
}

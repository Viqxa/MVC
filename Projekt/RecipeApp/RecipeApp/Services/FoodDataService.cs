using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RecipeApp.Services.DTOs;

namespace RecipeApp.Services
{
    public class FoodDataService : IFoodDataService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly ILogger<FoodDataService> _logger;

        public FoodDataService(
            HttpClient client,
            IConfiguration config,
            ILogger<FoodDataService> logger)
        {
            _client = client;
            _logger = logger;
            _apiKey = config["Fdc:ApiKey"]
               ?? throw new InvalidOperationException("FDC API key not configured.");
            var baseUrl = (config["Fdc:BaseUrl"] ?? throw new InvalidOperationException("FDC BaseUrl not configured."))
                          .TrimEnd('/') + "/";
            _client.BaseAddress = new Uri(baseUrl);

            _logger.LogInformation("FoodDataService BaseUrl={BaseUrl}", baseUrl);
        }

        public async Task<IEnumerable<FoodItemDto>> SearchAsync(string query)
        {
            var url = $"foods/search?api_key={_apiKey}";
            var payload = new { query };

            var resp = await _client.PostAsJsonAsync(url, payload);
            resp.EnsureSuccessStatusCode();

            var search = await resp.Content
                                   .ReadFromJsonAsync<SearchResponse>();
            return search?.Foods
                         .Select(f => new FoodItemDto
                         {
                             FdcId = f.FdcId,
                             Description = f.Description
                         })
                   ?? Enumerable.Empty<FoodItemDto>();
        }

        public async Task<FoodDetailDto> GetDetailsAsync(int fdcId)
        {
            _logger.LogInformation("Fetching details for FDC ID {FdcId}", fdcId);

            // 1) Fetch the raw food record
            var detail = await _client
                .GetFromJsonAsync<FoodDetailResponse>(
                    $"food/{fdcId}?api_key={_apiKey}")
                ?? throw new InvalidOperationException(
                    $"No details for FDC ID {fdcId}");

            // 2) Try to find the Energy nutrient (#208)
            var energyEntry = detail.FoodNutrients
                .FirstOrDefault(fn => fn.Nutrient.Number == "208");

            // 3) If missing (e.g. branded food), fall back to Foundation search
            if (energyEntry == null)
            {
                _logger.LogWarning(
                  "No 208/Energy for {FdcId} – falling back to Foundation search",
                  fdcId);

                // Search by description, restricted to 'Foundation' data type
                var fb = new
                {
                    query = detail.Description,
                    dataType = new[] { "Foundation" }
                };
                var fallbackResp = await _client
                    .PostAsJsonAsync($"foods/search?api_key={_apiKey}", fb);

                if (fallbackResp.IsSuccessStatusCode)
                {
                    var fbResult = await fallbackResp.Content
                                                      .ReadFromJsonAsync<SearchResponse>();
                    var first = fbResult?.Foods.FirstOrDefault();
                    if (first != null)
                    {
                        _logger.LogInformation(
                          "Foundation fallback got FDC {NewId} for '{Desc}'",
                          first.FdcId, detail.Description);

                        detail = await _client
                            .GetFromJsonAsync<FoodDetailResponse>(
                                $"food/{first.FdcId}?api_key={_apiKey}")
                            ?? detail;

                        energyEntry = detail.FoodNutrients
                            .FirstOrDefault(fn => fn.Nutrient.Number == "208");
                    }
                }
            }

            var kcalPer100g = energyEntry?.Amount ?? 0;
            _logger.LogInformation(
                "FDC {FdcId} ENERGY#{Num} = {Kcal} kcal/100g",
                fdcId,
                energyEntry?.Nutrient.Number ?? "(none)",
                kcalPer100g);

            return new FoodDetailDto
            {
                FdcId = detail.FdcId,
                Description = detail.Description,
                CaloriesPer100g = kcalPer100g
            };
        }

    

        private record SearchResponse(
            int CurrentPage,
            List<FoodDto> Foods
        );

        private record FoodDto(
            int FdcId,
            string Description
        );

        private record FoodDetailResponse(
            int FdcId,
            string Description,
            List<FoodNutrientDto> FoodNutrients
        );

        private record FoodNutrientDto(
            [property: System.Text.Json.Serialization.JsonPropertyName("nutrient")]
            NutrientInfo Nutrient,
            [property: System.Text.Json.Serialization.JsonPropertyName("amount")]
            double       Amount
        );

        private record NutrientInfo(
            [property: System.Text.Json.Serialization.JsonPropertyName("id")]
            int    Id,
            [property: System.Text.Json.Serialization.JsonPropertyName("number")]
            string Number,
            [property: System.Text.Json.Serialization.JsonPropertyName("name")]
            string Name,
            [property: System.Text.Json.Serialization.JsonPropertyName("unitName")]
            string Unit
        );
    }
}

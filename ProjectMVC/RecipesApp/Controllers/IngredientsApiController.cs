using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace RecipesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly string _apiKey;

        public IngredientsApiController(
            IHttpClientFactory httpFactory,
            IConfiguration config)
        {
            _httpFactory = httpFactory;
            // 2. Read the API key from the "Fdc" section
            _apiKey = config["Fdc:ApiKey"]!;
        }

        // GET /api/ingredientsapi/search?q=apple
        [HttpGet("search")]
        public async Task<IActionResult> Search(string q)
        {

            var client = _httpFactory.CreateClient("Fdc");

            
            var response = await client.GetFromJsonAsync<FoodSearchResponse>(
                $"foods/search?api_key={_apiKey}&query={Uri.EscapeDataString(q)}"
            );

            if (response == null)
                return NotFound();


            var results = response.Foods.Select(f => new {
                f.FdcId,
                Name = f.Description,

                Kcal = f.FoodNutrients
                          .FirstOrDefault(n => n.NutrientName == "Energy")
                          ?.Value ?? 0
            });

            return Ok(results);
        }
    }
}

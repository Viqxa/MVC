using RecipeApp.Services.DTOs;

namespace RecipeApp.Services
{
    public interface IFoodDataService
    {
        Task<IEnumerable<FoodItemDto>> SearchAsync(string query);
        Task<FoodDetailDto> GetDetailsAsync(int fdcId);
    }
}
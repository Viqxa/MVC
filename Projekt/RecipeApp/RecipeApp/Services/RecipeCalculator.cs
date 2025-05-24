using RecipeApp.Services;

public class RecipeCalculator
{
    private readonly IFoodDataService _foodSvc;
    private readonly ILogger<RecipeCalculator> _logger;

    public RecipeCalculator(
        IFoodDataService foodSvc,
        ILogger<RecipeCalculator> logger)
    {
        _foodSvc = foodSvc;
        _logger = logger;
    }

    public async Task<double> CalculateTotalCaloriesAsync(
        IEnumerable<(int fdcId, double grams)> ingredients)
    {
        double total = 0;
        foreach (var (id, grams) in ingredients)
        {
            var detail = await _foodSvc.GetDetailsAsync(id);
            _logger.LogInformation(
              "API returned {kcalPer100g} kcal/100g for FdcId={id}",
               detail.CaloriesPer100g, id);

            var itemKcal = (detail.CaloriesPer100g / 100.0) * grams;
            _logger.LogInformation(
              " -> scaled by {grams}g = {itemKcal} kcal",
               grams, itemKcal);

            total += itemKcal;
        }
        _logger.LogInformation("Total recipe kcal = {total}", total);
        return total;
    }
}

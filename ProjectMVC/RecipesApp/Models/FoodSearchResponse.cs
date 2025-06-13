using System.Collections.Generic;

public class FoodSearchResponse
{
    public List<FoodItem> Foods { get; set; } = new();
}

public class FoodItem
{
    public int FdcId { get; set; }
    public string Description { get; set; } = "";
    public List<FoodNutrient> FoodNutrients { get; set; } = new();
}

public class FoodNutrient
{
    public string NutrientName { get; set; } = "";
    public decimal Value { get; set; }
}

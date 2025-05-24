using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApp.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; set; }

        public int FdcId { get; set; }

        public string Description { get; set; }

        public double WeightInGrams { get; set; }
    }
}
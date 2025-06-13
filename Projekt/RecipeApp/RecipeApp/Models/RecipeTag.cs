﻿namespace RecipeApp.Models
{
    public class RecipeTag
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;
        public string Tag { get; set; } = string.Empty;
    }
}
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace applications_exe3.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURl { get; set; }
        public string cookingMethod { get; set; }
        public double Time { get; set; }

        // Get all recipes from DB
        public static List<Recipe> getRecipes()
        {
            DBservices db = new DBservices();
            return db.ReadRecipes();
        }

        // Add new recipe to DB
        public int Insert()
        {
            DBservices db = new DBservices();
            return db.InsertRecipe(this);
        }

        // Add new ingredient to recipe
        public static int InsertIngredientsToRecipe(int recipeid ,int [] ingredientIds)
        {
            DBservices db = new DBservices();
            return db.InsertIngredientsToRecipe(recipeid, ingredientIds);
        }
    }

}

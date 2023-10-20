namespace applications_exe3.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURl { get; set; }
        public double Calories { get; set; }

        // Get all ingredients from DB
        public static List<Ingredient> ReadIngredients()
        {
            DBservices db = new DBservices();
            return db.ReadIngredients();
        }

        // Get ingredient by ID
        public static List<Ingredient> ReadIngredientsByID(int id)
        {
            DBservices db = new DBservices();
            return db.ReadIngredientsByID(id);
        }

        // Add new ingredient to DB
        public int Insert()
        {
            DBservices db = new DBservices();
            return db.InsertIngredient(this);
        }
    }
}

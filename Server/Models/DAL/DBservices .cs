using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using applications_exe3.Models;

/// <summary>
/// DBServices is a class created by me to provide some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {
        // Read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts ingredient to the ingredients table 
    //--------------------------------------------------------------------------------------------------
    public int InsertIngredient(Ingredient ingredient)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // Create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        cmd = CreateInsertIngredientCommandWithStoredProcedure("spAddIngredient", con, ingredient);     
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // Execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // Close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts ingredient to the ingredients table 
    //--------------------------------------------------------------------------------------------------
    public int InsertRecipe(Recipe recipe)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // Create the connection
        }
        catch (Exception ex)
        {
            // Write to log
            throw (ex);
        }
        cmd = CreateInsertRecipeCommandWithStoredProcedure("spAddRecipe", con, recipe);       
        try
        {
            int id = Convert.ToInt32(cmd.ExecuteScalar()); // Execute the command
            return id;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts ingredients to the recipe  
    //--------------------------------------------------------------------------------------------------
    public int InsertIngredientsToRecipe(int recipeid, int[] ingredientid)
    {
        SqlConnection con;
        SqlCommand cmd;
        int totalNumEffected = 0;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        try
        {
            foreach (var item in ingredientid)
            {
                cmd = CreateInsertIngredientToRecipeCommandWithStoredProcedure("spAddIngredientToRecipe", con, recipeid, item);
                int numEffected = cmd.ExecuteNonQuery();
                totalNumEffected += numEffected;
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
        return totalNumEffected;
    }

    //--------------------------------------------------------------------------------------------------
    // This method reads all the recipes
    //--------------------------------------------------------------------------------------------------
    public List<Recipe> ReadRecipes()
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        cmd = CreateReadRecipesCommandWithStoredProcedure("spGetRecipes", con);  
        List<Recipe> list = new List<Recipe>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                Recipe rec = new Recipe();
                rec.Id = Convert.ToInt32(dataReader["Id"]);
                rec.Name = dataReader["Name"].ToString();
                rec.ImageURl = dataReader["ImageURL"].ToString();
                rec.cookingMethod = dataReader["CookingMethod"].ToString();
                rec.Time = Convert.ToDouble(dataReader["Time"]);
                list.Add(rec);
            }
            return list;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method reads all the ingredients
    //--------------------------------------------------------------------------------------------------
    public List<Ingredient> ReadIngredients()
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        cmd = CreateReadIngredientsCommandWithStoredProcedure("spGetIngredients", con);  
        List<Ingredient> list = new List<Ingredient>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                Ingredient ing = new Ingredient();
                ing.Id = Convert.ToInt32(dataReader["id"]);
                ing.Name = dataReader["Name"].ToString();
                ing.ImageURl = dataReader["ImageURL"].ToString();
                ing.Calories = Convert.ToDouble(dataReader["Calories"]);
                list.Add(ing);
            }
            return list;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method reads ingredient by recipe id 
    //--------------------------------------------------------------------------------------------------
    public List<Ingredient> ReadIngredientsByID(int id)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateReadIngredientsByIDCommandWithStoredProcedure("spGetIngredientsByRecipeID", con, id);
        List<Ingredient> list = new List<Ingredient>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                Ingredient ing = new Ingredient();
                ing.Id = Convert.ToInt32(dataReader["Id"]);
                ing.Name = dataReader["Name"].ToString();
                ing.ImageURl = dataReader["ImageURL"].ToString();
                ing.Calories = Convert.ToDouble(dataReader["Calories"]);
                list.Add(ing);
            }
            return list;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommands
    //---------------------------------------------------------------------------------
    private SqlCommand CreateReadRecipesCommandWithStoredProcedure(string spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object
        cmd.Connection = con;              // Assign the connection to the command object
        cmd.CommandText = spName;      // Can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be stored procedure
        return cmd;
    }

    private SqlCommand CreateInsertIngredientCommandWithStoredProcedure(string spName, SqlConnection con, Ingredient ingredient)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object
        cmd.Connection = con;              // Assign the connection to the command object
        cmd.CommandText = spName;      // Can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be stored procedure
        cmd.Parameters.AddWithValue("@Name", ingredient.Name);
        cmd.Parameters.AddWithValue("@ImageURL", ingredient.ImageURl);
        cmd.Parameters.AddWithValue("@Calories", ingredient.Calories);
        return cmd;
    }

    private SqlCommand CreateInsertIngredientToRecipeCommandWithStoredProcedure(string spName, SqlConnection con, int recipeid, int ingredientid) {
        SqlCommand cmd = new SqlCommand(); // Create the command object
        cmd.Connection = con;              // Assign the connection to the command object
        cmd.CommandText = spName;      // Can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be stored procedure
        cmd.Parameters.AddWithValue("@recipeid", recipeid);
        cmd.Parameters.AddWithValue("@ingredientid", ingredientid);
        return cmd;
    }

    private SqlCommand CreateInsertRecipeCommandWithStoredProcedure(string spName, SqlConnection con, Recipe recipe)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object
        cmd.Connection = con;              // Assign the connection to the command object
        cmd.CommandText = spName;      // Can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be stored procedure
        cmd.Parameters.AddWithValue("@Name", recipe.Name);
        cmd.Parameters.AddWithValue("@ImageURL", recipe.ImageURl);
        cmd.Parameters.AddWithValue("@CookingMethod", recipe.cookingMethod);
        cmd.Parameters.AddWithValue("@Time", recipe.Time);
        return cmd;
    }

    private SqlCommand CreateReadIngredientsCommandWithStoredProcedure(string spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object
        cmd.Connection = con;              // Assign the connection to the command object
        cmd.CommandText = spName;      // Can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be stored procedure
        return cmd;
    }

    private SqlCommand CreateReadIngredientsByIDCommandWithStoredProcedure(string spName, SqlConnection con, int id)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object
        cmd.Connection = con;              // Assign the connection to the command object
        cmd.CommandText = spName;      // Can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be stored procedure
        cmd.Parameters.AddWithValue("@recipeid", id);
        return cmd;
    }
}

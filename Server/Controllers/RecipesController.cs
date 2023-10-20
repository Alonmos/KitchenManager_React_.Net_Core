using Microsoft.AspNetCore.Mvc;
using applications_exe3.Models;
using System.Net.NetworkInformation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace applications_exe3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        // GET: api/<RecipesController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Recipe.getRecipes());
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Error at getting Recipes" + ex.Message);
            }
        }

        // POST api/<RecipesController>
        [HttpPost]
        public IActionResult Post([FromBody] Recipe rec)
        {
            try
            {
                int id = rec.Insert();
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error at posting Recipe" + ex.Message);
            }
        }

        // POST api/<RecipesController>
        [HttpPost("recipeid/{recipeid}")]
        public IActionResult PostIngredientsToRecipe(int recipeid, int [] ingredientIds)
        {
            try
            {
                int numaffected = Recipe.InsertIngredientsToRecipe(recipeid, ingredientIds);
                return Ok(numaffected);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error at posting ingredient to Recipe" + ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using applications_exe3.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace applications_exe3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        // GET: api/<IngredientsController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok (Ingredient.ReadIngredients());
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Error at getting Ingredients" + ex.Message);
            }

        }

        // GET api/<IngredientsController>/5
        [HttpGet("id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(Ingredient.ReadIngredientsByID(id));
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Error at getting Ingredient" + ex.Message);
            }
        }

        // POST api/<IngredientsController>
        [HttpPost]
        public IActionResult Post([FromBody] Ingredient ing)
        {
            try
            {
                int numaffected = ing.Insert();
                return Ok(numaffected);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error at posting Ingredient" + ex.Message);
            }

        }
    }
}

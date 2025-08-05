using BusinessLayer.Managers.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindingController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOne(int id, string name) // Primitive Types ==> Query Parameters
        {
            return Ok($"Id = {id}, Name = {name}");
        }

        [HttpGet("{id:int}/{name:alpha}")]
        public IActionResult GetTwo(int id , string name)   // Primitive Types ==> Path Variables
        {
            return Ok($"Id = {id}, Name = {name}");
        }

        [HttpGet("{id:int}")]
        public IActionResult GetThree(int id, string name)   // Primitive Types ==> Path Variables , Query Parameters
        //public IActionResult GetThree([FromRoute] int id, [FromQuery] string name)
        {
            return Ok($"Id = {id}, Name = {name}");
        }

        [HttpPost("{id:int}/{name:alpha}")]
        public IActionResult PostOne(int id, string name, [FromForm] Instructor instructor)
        {
            return Ok($"Instructor Id = {instructor.id} \n Instructor Name = {instructor.name}");
        }

        // Dependency Injection through Method Injection
        [HttpGet("methodinjection/{id:int}")]
        public async Task<IActionResult> UseMethodInjection(int id, [FromServices] IProductManager productManager)
        {
            return Ok(await productManager.GetById(id));
        }

        [HttpGet("useheader")]
        public async Task<IActionResult> UseHeader([FromHeader] int id)
        {
            throw new Exception("My CUstom Exception");
            return Ok($"Id = {id}");
        }

        // Complex Type  from Route

        [HttpGet("complex/{id:int}/{name:alpha}")]
        //public async Task<IActionResult> ComplexFromRoute([FromRoute] int id, [FromRoute] string name)  // ==> Primitive ( Default Behaviour )
        public async Task<IActionResult> ComplexFromRoute([FromRoute] Instructor instructor)
        {
            return Ok($"Id = {instructor.id}, Name = {instructor.name}");
        }
    }

    public class Instructor
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}

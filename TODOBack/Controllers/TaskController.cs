using Microsoft.AspNetCore.Mvc;
using TODOBack.Data;
using TODOBack.Models;

namespace PlanejaiBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        [Route("/Tasks/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Tasks!.OrderBy(e => e.Status).ToList());
        }

        [HttpGet("/Tasks/{id:int}")]
        public IActionResult GetByID([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var taskModel = context.Tasks!.SingleOrDefault(e => e.TaskId == id);

            if (taskModel == null)
            {
                return NotFound();
            }

            return Ok(taskModel);
        }

        [HttpGet("/TasksByUser/{id:int}")]
        public IActionResult GetByUser([FromRoute] int id, [FromServices] AppDbContext context)
        {
            return Ok(context.Tasks!.Where(e => e.Organizer!.UserId == id).OrderBy(e => e.Status).ToList());
        }

        [HttpPost("/Tasks/")]
        public IActionResult Post([FromBody] TaskModel taskModel, [FromServices] AppDbContext context)
        {
            var existingTask = context.Tasks!.SingleOrDefault(e => e.TaskId == taskModel.TaskId);

            if (existingTask == null)
            {
                context.Tasks!.Add(taskModel);
                context.SaveChanges();

                return Created($"/{taskModel.TaskId}", taskModel);
            }

            return BadRequest("A tarefa já foi criada!");
        }

        [HttpPut("/EditTask/{id:int}")]
        public IActionResult Put([FromRoute] int id, [FromBody] TaskModel taskModel, [FromServices] AppDbContext context)
        {
            if (id == taskModel.TaskId)
            {
                context.Tasks!.Update(taskModel);
                context.SaveChanges();

                return Ok(taskModel);
            }

            return NotFound();
        }

        [HttpDelete("/Tasks/{taskId:int}")]
        public IActionResult Delete([FromRoute] int taskId, [FromServices] AppDbContext context)
        {
            var taskModel = context.Tasks!.Find(taskId);

            if (taskModel != null)
            {
                context.Tasks!.Remove(taskModel);
                context.SaveChanges();

                return Ok(taskModel);
            }

            return NotFound("A tarefa já foi removida.");
        }
    }
}

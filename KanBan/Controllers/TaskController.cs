using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KanBan.Data;
using KanBan.Models;
using KanBan.Models.Dtos;

namespace KanBan.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TaskController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColumnTask>>> GetAllTasks()
        {
            var tasks = await _db.ColumnTasks.ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("task={id}")]
        public async Task<ActionResult<ColumnTask>> GetTask(int id)
        {
            var task = await _db.ColumnTasks.FindAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            var newTask = new ColumnTask
            {
                Title = dto.Title,
                ColumnId = dto.ColumnId
            };
            _db.ColumnTasks.Add(newTask);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask);
        }

        [HttpPatch("task={id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            var task = await _db.ColumnTasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = dto.Title ?? task.Title;
            await _db.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("task={id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _db.ColumnTasks.FindAsync(id);
            if (task == null) return NotFound();

            _db.ColumnTasks.Remove(task);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}

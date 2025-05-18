using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using TestASpAPI.Data;
using TestASpAPI.Models;
using TestASpAPI.Models.Dtos;

namespace TestASpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ColumnController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("boards={boardId}/columns")]   
        public async Task<ActionResult<IEnumerable<Column>>> GetColumnsByBoard(int boardId)
        {
            var columns = await _db.Columns
                .Where(c => c.BoardId == boardId)
                .Include(c => c.Tasks)
                .ToListAsync();

            return Ok(columns);
        }

        [HttpPost("boards={boardId}/columns")]
        public async Task<ActionResult> CreateColumn(int boardId, [FromBody] BoardColumnDto dto)
        {
            var boardExists = await _db.Boards.AnyAsync(b => b.Id == boardId);
            if (!boardExists) return NotFound("Board not found");

            var newColumn = new BoardColumn
            {
                BoardId = boardId,
                Title = dto.Title,
            };
      
            _db.Columns.Add(newColumn);
            await _db.SaveChangesAsync();

            //return Ok(new { message = "Column created", column.Id });
            return Content($"Board {boardId} add Column {dto.Title} success.");
        }

        [HttpPatch("column={id}")]
        public async Task<IActionResult> UpdateColumn(int id, [FromBody] BoardColumnDto dto)
        {
            var column = await _db.Columns.FindAsync(id);
            if (column == null) return NotFound("Column not found");


            column.Title = dto.Title ?? column.Title;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Column updated" });
        }

        [HttpDelete("column={id}")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            var column = await _db.Columns.FindAsync(id);
            if (column == null) return NotFound("Column not found");

            _db.Columns.Remove(column);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Column deleted" });
        }

    }
}

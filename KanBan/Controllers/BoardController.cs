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
    public class BoardController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BoardController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Board>> Get()
        {
            var boards = _db.Boards
                .Include(b => b.Members).ThenInclude(m => m.User)
                .Include(b => b.Creator)
                .ToList();
            if (boards == null || !boards.Any())
                return NotFound();

            var boardDtos = boards.Select(b => new BoardDto
            {
                Id = b.Id,
                Title = b.Name,
                CreatorId = b.Creator.Id,
                Members = b.Members.Select(m => new UserSimpleDto
                {
                    Id = m.User.Id,
                    Username = m.User.Username,
                    Email = m.User.Email,
                    Role = m.Role
                }).ToList(),
            }).ToList();

            return Ok(boardDtos);
        }

        [HttpGet("board={id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
            var board = await _db.Boards
                .Include(b => b.Members).ThenInclude(m => m.User)
                .Include(b => b.Creator)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (board == null) return NotFound();

            var boardDto = new BoardDto
            {
                Id = board.Id,
                Title = board.Name,
                CreatorId = board.Creator.Id,
                Members = board.Members.Select(m => new UserSimpleDto
                {
                    Id = m.User.Id,
                    Username = m.User.Username,
                    Email = m.User.Email,
                    Role = m.Role
                }).ToList()
            };
            return Ok(boardDto);
        }

        [HttpPost]
        public async Task<ActionResult<Board>> CreateBoard([FromBody] CreateBoardDto dto)
        {
            //dto.Board.CreatedAt  = DateTime.UtcNow.AddHours(7);


            var newBoard = new Board
            {
                Name = dto.Title,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow.AddHours(7)
            };

            _db.Boards.Add(newBoard);
            await _db.SaveChangesAsync();

            var newBoardM = new BoardMember
            {
                BoardId = newBoard.Id,
                UserId = dto.CreatedBy,
                Role = "Owner",

            };

            _db.BoardMembers.Add(newBoardM);
            await _db.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetBoard), new { id = newBoard.Id }, newBoard);
            return Content("add Board Success");
        }

        [HttpPatch("board={id}")]
        public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardDto dto)
        {
            var board = await _db.Boards.FindAsync(id);
            if (board == null)
                return NotFound("Board not found");

            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                board.Name = dto.Title;
            }


            await _db.SaveChangesAsync();

            return Content("Patch Success");
        }

        [HttpDelete("board={id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var board = await _db.Boards
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (board == null)
                return NotFound("Board not found");

            _db.BoardMembers.RemoveRange(board.Members);

            _db.Boards.Remove(board);
            await _db.SaveChangesAsync();

            return Ok($"Board {id} deleted successfully.");
        }


        [HttpPost("board={boardId}/members")]
        public async Task<IActionResult> AddMember(int boardId, [FromBody] AddMemberDto dto )
        {
            var board = await _db.Boards.FindAsync(boardId);
            if (board == null) return NotFound("Board not found");

            var nMember = new BoardMember
            {
                BoardId = boardId,
                UserId = dto.UserId,
                Role = dto.Role,
            };
            _db.BoardMembers.Add(nMember);
            await _db.SaveChangesAsync();

            return Content($"Add member {dto.UserId} to {boardId} success");
        }

        [HttpDelete("board={boardId}/members={userId}")]
        public async Task<IActionResult> RemoveMember(int boardId, int userId)
        {
            var member = await _db.BoardMembers
                .FirstOrDefaultAsync(m => m.BoardId == boardId && m.UserId == userId);

            if (member == null)
                return NotFound("Member not found in this board");

            _db.BoardMembers.Remove(member);
            await _db.SaveChangesAsync();

            return Ok($"Removed user {userId} from board {boardId} successfully");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestASpAPI.Models;
using TestASpAPI.Data;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TestASpAPI.Models.Dtos;

namespace TestASpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {

            var users = _db.Users
                .Include(u => u.CreatedBoards).ThenInclude(b => b.Members).ThenInclude(m => m.User)
                .ToList();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedBoards = u.CreatedBoards.Select(b => new BoardDto
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
                        
                    }).ToList()
                }).ToList()
            }).ToList();

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }

            return Ok(userDtos);
        }

        [HttpGet("user={id}")]
        public ActionResult GetUser(int id)
        {
            var user = _db.Users
                .Where(u => u.Id == id)
                .Select(u => new {
                    u.Id,
                    u.Username,
                    u.Email,
                    CreatedBoards = u.CreatedBoards.Select(b => new {
                        b.Id,
                        b.Name,
                        b.CreatedAt
                    }),
                    BoardMemberships = u.BoardMemberships.Select(m => new {
                        m.Role,
                        Board = new
                        {
                            m.Board.Id,
                            m.Board.Name
                        }
                    })
                })
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_db.Users.Any(u => u.Email == dto.Email))
                return Conflict("Email already exists.");

            if (_db.Users.Any(u => u.Username == dto.Username))
                return Conflict("Username already exists.");

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow.AddHours(7)
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }



        [HttpPatch("user={id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Username))
                user.Username = dto.Username;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("user={id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return Content("Delete id Success"); 
        }




    }
}

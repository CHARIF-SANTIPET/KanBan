using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestASpAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "*username")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*Email")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*password")]
        [JsonIgnore]
        [Column("Password_hash")]
        public string Passwordhash{ get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);

        public ICollection<Board> CreatedBoards { get; set; } =  new List<Board>();

        public ICollection<BoardMember> BoardMemberships { get; set; } = new List<BoardMember>();

    }
}

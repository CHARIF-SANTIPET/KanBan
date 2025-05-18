using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KanBan.Models
{
    public class BoardMember
    {
        public int Id { get; set; }

        public int BoardId { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string Role { get; set; } 
    }
}

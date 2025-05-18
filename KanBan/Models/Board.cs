using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KanBan.Models
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public User? Creator { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<BoardMember> Members { get; set; } = new List<BoardMember>();
        public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();

    }
}

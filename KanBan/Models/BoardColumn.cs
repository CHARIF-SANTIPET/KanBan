using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KanBan.Models
{
    public class BoardColumn
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int BoardId { get; set; }
        [ForeignKey("BoardId")]
        [JsonIgnore]
        public Board? Board { get; set; }

        public ICollection<ColumnTask> Tasks { get; set; } = new List<ColumnTask>();
    }
}

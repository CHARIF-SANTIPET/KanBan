using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KanBan.Models
{
    public class ColumnTask
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int ColumnId { get; set; }

        [ForeignKey("ColumnId")]
        [JsonIgnore]
        public BoardColumn BoardColumn { get; set; }
    }
}

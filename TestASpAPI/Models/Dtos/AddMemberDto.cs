namespace TestASpAPI.Models.Dtos
{
    public class AddMemberDto
    {
        public int UserId { get; set; }
        public int BoardId { get; set; }
        public string Role { get; set; }
    }
}

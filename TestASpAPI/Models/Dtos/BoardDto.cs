namespace TestASpAPI.Models.Dtos
{
    public class BoardDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public List<UserSimpleDto> Members { get; set; }
    }
}

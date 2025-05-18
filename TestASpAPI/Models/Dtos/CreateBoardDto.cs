namespace TestASpAPI.Models.Dtos
{
    public class CreateBoardDto
    {
        public int CreatedBy { get; set; }
        public string Title { get; set; }
        public List<CreateBoardMembershipDto> Members { get; set; }
    }
}

namespace MR_dw2.Models.NewFolder
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }
    }
}

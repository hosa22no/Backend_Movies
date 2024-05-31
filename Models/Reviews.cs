namespace MR_dw2.Models
{
    public class Review
    {
        
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        // Foreign key to create a connection to the Movies table
        public int MovieId { get; set; }

        // Navigation property 
        public Movies Movie { get; set; }
    }
}

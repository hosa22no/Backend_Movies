using System.ComponentModel.DataAnnotations;

namespace MR_dw2.Models
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public int ReleaseYear { get; set; }
        public string? Description { get; set; }

        //Navigation property representing the relationship between a movie and their review.
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

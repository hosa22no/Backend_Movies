using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_dw2.Models
{
    public class Review
    {
       
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        // Foreign key to create a connection to the Movies table
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movies Movie { get; set; }
        

    }
}

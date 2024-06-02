using Microsoft.AspNetCore.Identity;

namespace MR_dw2.Models
    

    //A user should be connected to their reviews.
{
    public class User : IdentityUser
    {
        public ICollection<Review>? Reviews { get; set; }
    }
}

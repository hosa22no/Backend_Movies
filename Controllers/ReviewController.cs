using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR_dw2.Models;
using MR_dw2.Data;
using System.Threading.Tasks;
using MR_dw2.Models.NewFolder;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ApplicationDbContext db;

    public ReviewController(ApplicationDbContext db)
    {
        this.db = db;
    }

    // Create a review by a logged in user
    // POST: api/reviews
    [HttpPost, Authorize]
    public async Task<ActionResult<Review>> PostReview(ReviewDTO reviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user id

        var review = new Review
        {
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            MovieId = reviewDto.MovieId,
            UserId = userId //Connect the user to the review
        };

        db.Reviews.Add(review);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
    }


    // GET: api/reviews/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = await db.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        return review;
    }

    // PUT: api/Review/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] Review updatedReview)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var review = await db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        // Update the review
        db.Entry(review).CurrentValues.SetValues(updatedReview);
        await db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Review/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        db.Reviews.Remove(review);
        await db.SaveChangesAsync();

        return NoContent();
    }

    // Get all reviews for a specific movie
    // GET: api/reviews/movie/{movieId}
    [HttpGet("movies/{movieId}/reviews")]
    public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByMovie(int movieId)
    {
        var reviews = await db.Reviews
            .Where(r => r.MovieId == movieId)
            .Include(r => r.User) // Include user details if needed
            .Select(r => new ReviewDTO
            {
               
                MovieId = r.MovieId,
                Comment = r.Comment,
                Rating = r.Rating
            })
            .ToListAsync();

        return Ok(reviews);
    }

    //Get all reviews by a specific user
    // GET: api/reviews/user/{userId}
    [HttpGet("myreviews")]
    [Authorize] // Säkerställer att endast autentiserade användare kan hämta sina recensioner
    public async Task<IActionResult> GetMyReviews()
    {
        // Hämta den inloggade användarens ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Hämta alla recensioner kopplade till användaren och inkludera filmdata
        var reviews = await db.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.Movie) // Inkludera filmdata
            .Select(r => new ReviewDTO
            {
                Rating = r.Rating,
                Comment = r.Comment,
                MovieId = r.MovieId,
                MovieTitle = r.Movie.Title // Förutsatt att din Movie-entitet har en Title-egenskap
            })
            .ToListAsync();

        // Kontrollera om det finns några recensioner
        if (reviews.Any())
        {
            // Returnera recensionerna
            return Ok(reviews);
        }
        else
        {
            // Returnera ett meddelande om användaren inte har skrivit några recensioner
            return Ok(new { Message = "Du har inte skrivit några recensioner än." });
        }
    }



}

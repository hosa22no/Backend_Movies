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

    // GET: api/reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
    {
        var reviews = await db.Reviews.ToListAsync();

        if (reviews == null || !reviews.Any())
        {
            return NotFound(new { Message = "Inga recensioner hittades." });
        }

        return Ok(reviews);
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
    [HttpPut("{id}"), Authorize]
    
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDTO reviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var review = await db.Reviews.FirstOrDefaultAsync(r => r.Id == id && r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (review == null)
        {
            return NotFound();
        }

        // Ensure that the user updating the review is the one who created it
        if (review.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return Unauthorized();
        }

        // Update the review with the values from the DTO
        review.Rating = reviewDto.Rating;
        review.Comment = reviewDto.Comment;
        review.MovieId = reviewDto.MovieId;

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

    //Get all reviews by the logged in user
    // GET: api/reviews/user/{userId}
    [HttpGet("myreviews"), Authorize]
    
    public async Task<IActionResult> GetMyReviews()
    {
        // Get the user id
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get all reviews by the user
        var reviews = await db.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.Movie) 
            .Select(r => new ReviewDTO
            {
                Rating = r.Rating,
                Comment = r.Comment,
                MovieId = r.MovieId,
                MovieTitle = r.Movie.Title
            })
            .ToListAsync();

        // Check if the user has written any reviews
        if (reviews.Any())
        {

            return Ok(reviews);
        }
        else
        {
            // Returns a message if the user has not written any reviews
            return Ok(new { Message = "Du har inte skrivit några recensioner än." });
        }
    }



}

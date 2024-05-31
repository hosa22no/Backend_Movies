using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR_dw2.Models;
using MR_dw2.Data;
using System.Threading.Tasks;
using MR_dw2.Models.NewFolder;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ApplicationDbContext db;

    public ReviewController(ApplicationDbContext db)
    {
        this.db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Review>> PostReview(ReviewDTO reviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var review = new Review
        {
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            MovieId = reviewDto.MovieId
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
}

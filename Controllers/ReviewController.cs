using Microsoft.AspNetCore.Mvc;
using MR_dw2.Data;
using MR_dw2.Models;

namespace MR_dw2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public ReviewController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: api/Review
        [HttpGet]
        public IActionResult GetReviews()
        {
            return Ok(db.Reviews.ToList());
        }

        // GET: api/Review/{id}
        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = db.Reviews.FirstOrDefault(r => r.MovieId == id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // POST: api/Review
        [HttpPost]
        public IActionResult CreateReview([FromBody] Review review)
        {
            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }

        // PUT: api/Review/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Review updatedReview)
        {
            var review = db.Reviews.FirstOrDefault(r => r.MovieId == id);
            if (review == null)
            {
                return NotFound();
            }

            // Update the review
            db.Entry(review).CurrentValues.SetValues(updatedReview);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Review/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = db.Reviews.FirstOrDefault(r => r.MovieId == id);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return NoContent();
        }
    }
}

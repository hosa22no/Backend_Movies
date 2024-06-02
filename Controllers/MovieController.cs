using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR_dw2.Data;
using MR_dw2.Models;

namespace MR_dw2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        // Inject the ApplicationDbContext into the controller
        private readonly ApplicationDbContext db;

        // Constructor that takes an instance of ApplicationDbContext
        public MovieController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // Get all movies from the database
        [HttpGet, Authorize]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await db.Movies
                               //.Include(m => m.Reviews) 
                               .ToListAsync();
            return Ok(movies);
        }

        // Get a movie by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await db.Movies
                            // .Include(m => m.Reviews) Crashes the app
                             .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }


        // Create a new movie
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Movies movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        // Update a movie by id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movies updatedMovie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updatedMovie.Title;
            movie.ReleaseYear = updatedMovie.ReleaseYear;
            movie.Description = updatedMovie.Description;

            await db.SaveChangesAsync();

            return Ok(movie);
        }

        // Delete a movie by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}

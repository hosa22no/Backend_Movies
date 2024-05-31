using Microsoft.AspNetCore.Mvc;
using MR_dw2.Data;
using MR_dw2.Models;

namespace MR_dw2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        // Inject the ApplicationDbContext into the controller
        private readonly ApplicationDbContext db;

        // Constructor that takes an instance of ApplicationDbContext
        public MovieController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // Get all movies from the database
        [HttpGet]
        public IActionResult GetMovies()
        {
            return Ok(db.Movies.ToList());
        }
        // Get a movie by id
        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = db.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        // Create a new movie
        [HttpPost]
        public IActionResult CreateMovie([FromBody] Movies movie)
        {
            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        //Update a movie by id
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] Movies updatedMovie)
        {
            var movie = db.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updatedMovie.Title;
            movie.ReleaseYear = updatedMovie.ReleaseYear;
            movie.Description = updatedMovie.Description;

            db.SaveChanges();

            return Ok(movie);
        }

       // Delete a movie by id
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = db.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return NoContent();
        }
    }
} 



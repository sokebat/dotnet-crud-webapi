using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using videogameapi.Data;
using videogameapi.Model;

namespace videogameapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly VideoGameDbContext _context;
         private readonly ILogger<VideoGameController> _logger;  

        public VideoGameController(VideoGameDbContext context , ILogger<VideoGameController> logger )
        {
            _context = context;
            _logger = logger; 
        }

         
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            try
            {
                var videoGames = await _context.VideoGames.ToListAsync();
                 return Ok(new
                {
                     status = "success",
                     Message = "Successfully retrieved the video games.",
                    Data = videoGames
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving video games.");
                return StatusCode(500, "An error occurred while retrieving video games.");
            }
        }

         
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            try
            {
                if (id <= 0)
                {
                     _logger.LogWarning("Invalid ID {Id} provided for GetVideoGameById.", id); 
                    return BadRequest("Invalid ID.");
                }

                var videoGame = await _context.VideoGames.FindAsync(id);
                if (videoGame == null)
                {
                    _logger.LogWarning("Video game with ID {Id} not found.", id); 
                    return NotFound();
                }

                return Ok(new
                {
                    status = "success",
                    Message = "Successfully retrieved the video games.",
                    Data = videoGame
                });
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error retrieving video game with ID {Id}.", id); 
                return StatusCode(500, "An error occurred while retrieving the video game.");
            }
        }

         
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VideoGame>> AddVideoGame(VideoGame newVideoGame)
        {
            try
            {
                if (newVideoGame == null || !ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid video game data provided for creation."); 
                    return BadRequest(ModelState);
                }

                newVideoGame.Id = 0; // Ensure ID is reset for new entities
                _context.VideoGames.Add(newVideoGame);
                await _context.SaveChangesAsync();

                 _logger.LogInformation("Video game with ID {Id} created successfully.", newVideoGame.Id); 
                return CreatedAtAction(nameof(GetVideoGameById), new { id = newVideoGame.Id }, newVideoGame);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating video game."); 
                return StatusCode(500, "A database error occurred while creating the video game.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating video game.");  
                return StatusCode(500, "An error occurred while creating the video game.");
            }
        }

       
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVideoGame(int id, VideoGame updatedVideoGame)
        {
            try
            {
                if (id <= 0 || updatedVideoGame == null || !ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid ID {Id} or video game data provided for update.", id); 
                    return BadRequest("Invalid ID or video game data.");
                }

                if (id != updatedVideoGame.Id)
                {
                    _logger.LogWarning("ID {Id} in URL does not match video game ID {VideoGameId}.", id, updatedVideoGame.Id); 
                    return BadRequest("ID mismatch.");
                }

                var game = await _context.VideoGames.FindAsync(id);
                if (game == null)
                {
                    _logger.LogWarning("Video game with ID {Id} not found for update.", id);
                    return NotFound();
                }

                game.Title = updatedVideoGame.Title;
                game.Platform = updatedVideoGame.Platform;
                game.Publisher = updatedVideoGame.Publisher;
                game.Developer = updatedVideoGame.Developer;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Video game with ID {Id} updated successfully.", id); 

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while updating video game with ID {Id}.", id); 
                return StatusCode(500, "A concurrency error occurred while updating the video game.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating video game with ID {Id}.", id); 
                return StatusCode(500, "A database error occurred while updating the video game.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating video game with ID {Id}.", id); 
                return StatusCode(500, "An error occurred while updating the video game.");
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVideoGame(int id)
        {
            try
            {
                if (id <= 0)
                {
                     _logger.LogWarning("Invalid ID {Id} provided for deletion.", id); 
                    return BadRequest("Invalid ID.");
                }

                var game = await _context.VideoGames.FindAsync(id);
                if (game == null)
                {
                    _logger.LogWarning("Video game with ID {Id} not found for deletion.", id);
                    return NotFound();
                }

                _context.VideoGames.Remove(game);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Video game with ID {Id} deleted successfully.", id); 
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting video game with ID {Id}.", id);
                return StatusCode(500, "A database error occurred while deleting the video game.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting video game with ID {Id}.", id); 
                return StatusCode(500, "An error occurred while deleting the video game.");
            }
        }
    }
}
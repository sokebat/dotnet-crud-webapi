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

        public VideoGameController(VideoGameDbContext context)
        {
            _context = context;
        }   

        [HttpGet]
        //public ActionResult<List<VideoGame>> GetVideoGames()
        //{
        //    return Ok(videoGames);
        //} 

        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            var videoGames = await _context.VideoGames.ToListAsync();   
            return Ok(videoGames);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {

            var videogame = await _context.VideoGames.FindAsync(id);
            if (videogame == null)
                return NotFound();
            return Ok(videogame);
        }

        [HttpPost]

        public async Task<ActionResult<VideoGame>> AddVideoGame(VideoGame newVideogame)
        {
            if (newVideogame is null)
                return BadRequest();

                _context.VideoGames.Add(newVideogame);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetVideoGameById), new { id = newVideogame.Id }, newVideogame);
        }


        [HttpPut("{id}")]

        public async Task<IActionResult>  UpdateVideoGame(int id , VideoGame updatedVidoeGame)
        {

            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
                return NotFound();

            game.Title = updatedVidoeGame.Title;
            game.Platform = updatedVidoeGame.Platform;
            game.Publisher = updatedVidoeGame.Publisher;
            game.Developer = updatedVidoeGame.Developer;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);

            if (game is null)
                return NotFound();

            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}

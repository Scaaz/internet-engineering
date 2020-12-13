using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Wisieilec.API.Data;
using Wisieilec.Data.Entities;
using Wisieilec.Dtos;

namespace Wisieilec.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameController(
            DataContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/game/5/status
        [HttpGet("{gameId}/status")]
        public async Task<ActionResult<GameStatusDto>> GetStatus(int gameId)
        {
            var game = await GetGame(gameId);
            if (game == null)
            {
                return NotFound();
            }

            var word = "";
            foreach (var letter in game.Word.Name)
            {
                if (game.UsedLetters.Contains(letter.ToString()))
                {
                    word += letter;
                }
                else
                {
                    word += "_";
                }
            }

            return Ok(new GameStatusDto
            {
                Word = word,
                RemainingLives = game.RemainingLives,
                UsedLetters = game.UsedLetters
            });
        }

        // POST: api/game/5:guessLetter
        [HttpPost("{gameId}:guessLetter")]
        public async Task<ActionResult<GameStatusDto>> GuessLetter(int gameId, GuessLetterDto guessLetterDto)
        {
            var game = await GetGame(gameId);
            if (game == null || game.Lobby.Status == LobbyStatus.Finished)
            {
                return NotFound();
            }

            if (game.UsedLetters.Contains(guessLetterDto.Letter.ToString()))
            {
                throw new System.Exception("ALREADY GUESSED");
            }
            game.UsedLetters += guessLetterDto.Letter;

            if (game.Word.Name.Contains(guessLetterDto.Letter) == false)
            {
                game.RemainingLives--;
                if (game.RemainingLives <= 0)
                {
                    game.Lobby.Status = LobbyStatus.Finished;
                }
            }

            await _context.SaveChangesAsync();

            return await GetStatus(gameId);
        }

        // POST: api/game/5/user/2:guessWord
        [HttpPost("{gameId}:guessWord")]
        public async Task<ActionResult<GameStatusDto>> GuessWord(int gameId, GuessWordDto guessWordDto)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var game = await GetGame(gameId);
            if (game == null)
            {
                return NotFound();
            }

            //CHECK IF CAN GUESS WORD BECAUSE LIVES COULD BE DEPLETED

            if (game.Word.Name.Equals(guessWordDto.Word, System.StringComparison.OrdinalIgnoreCase))
            {
                game.Lobby.Status = LobbyStatus.Finished;
                var userWhoGuessed = game.Lobby.Users.FirstOrDefault(u => u.Id == userId);
                userWhoGuessed.TotalScore++;

                foreach (var user in game.Lobby.Users)
                {
                    user.LobbyId = null;
                }
            }
            else
            {
                game.RemainingLives--;
                if (game.RemainingLives <= 0)
                {
                    game.Lobby.Status = LobbyStatus.Finished;
                }
            }

            await _context.SaveChangesAsync();

            return await GetStatus(gameId);
        }

        private async Task<Game> GetGame(int gameId)
        {
            return await _context.Games
                .Include(g => g.Word)
                .Include(g => g.Lobby)
                    .ThenInclude(l => l.Users)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }
    }
}
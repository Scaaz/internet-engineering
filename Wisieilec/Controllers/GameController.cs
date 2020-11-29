using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wisieilec.API.Data;
using Wisieilec.Data.Entities;
using Wisieilec.Dtos;

namespace Wisieilec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;

        public GameController(DataContext context)
        {
            _context = context;
        }

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

        [HttpPost("{gameId}/user/{userId}:guessLetter")]
        public async Task<ActionResult<GameStatusDto>> GuessLetter(int gameId, int userId, GuessLetterDto guessLetterDto)
        {
            var game = await GetGame(gameId);
            if (game == null)
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

        [HttpPost("{gameId}/user/{userId}:guessWord")]
        public async Task<ActionResult<GameStatusDto>> GuessWord(int gameId, int userId, GuessWordDto guessWordDto)
        {
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
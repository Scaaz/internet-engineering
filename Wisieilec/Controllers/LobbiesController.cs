using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wisieilec.API.Data;
using Wisieilec.Data.Entities;
using Wisieilec.Dtos;

namespace Wisieilec.Controllers
{
    [Route("api/lobbies")]
    [ApiController]
    public class LobbiesController : ControllerBase
    {
        private readonly DataContext _context;

        public LobbiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Lobbies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetLobbies()
        {
            return await _context.Lobbies.ToListAsync();
        }

        // GET: api/Lobbies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lobby>> GetLobby(int id)
        {
            var lobby = await _context.Lobbies.FindAsync(id);

            if (lobby == null)
            {
                return NotFound();
            }

            return lobby;
        }

        // PUT: api/Lobbies/5
        [HttpPut("{lobbyId}/users/{userId}")]
        public async Task<IActionResult> JoinLobby(int lobbyId, int userId)
        {
            if (CanJoinLobby(lobbyId))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                user.LobbyId = lobbyId;
            }
            else
            {
                throw new Exception("USER CANT JOIN THIS LOBBY");
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Lobbies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lobby>> CreateLobby(LobbyDto lobbyDto)
        {
            var lobby = new Lobby
            {
                NumberOfLives = lobbyDto.NumberOfLives,
                LobbySize = lobbyDto.LobbySize,
                Status = LobbyStatus.New
            };

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLobby", new { id = lobby.Id }, lobby);
        }

        [HttpPost("{lobbyId}:start")]
        public async Task<ActionResult<Lobby>> StartLobby(int lobbyId)
        {
            //CHECK LOBBY STATUS BEFORE STARTING
            var lobby = await _context.Lobbies.FirstOrDefaultAsync(l => l.Id == lobbyId);
            lobby.Status = LobbyStatus.Pending;

            int total = _context.Words.Count();
            Random r = new Random();
            int offset = r.Next(0, total);

            var word = await _context.Words.Skip(offset).FirstOrDefaultAsync();
            var game = new Game
            {
                Lobby = lobby,
                Word = word,
                RemainingLives = lobby.NumberOfLives,
                UsedLetters = ""
            };

            _context.Set<Game>().Add(game);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Lobbies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lobby>> DeleteLobby(int id)
        {
            var lobby = await _context.Lobbies.FindAsync(id);
            if (lobby == null)
            {
                return NotFound();
            }

            _context.Lobbies.Remove(lobby);
            await _context.SaveChangesAsync();

            return lobby;
        }

        private bool CanJoinLobby(int lobbyId)
        {
            return _context.Lobbies.Any(l =>
                l.Id == lobbyId &&
                l.Status == LobbyStatus.New &&
                l.Users.Count() < l.LobbySize);
        }
    }
}
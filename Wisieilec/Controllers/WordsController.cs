using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Wisieilec.API.Data;
using Wisieilec.Data.Entities;
using Wisieilec.Dtos;

namespace Wisieilec.Controllers
{
    [Route("api/words")]
    [ApiController]
    [Authorize]
    public class WordsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WordsController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// TO JEST METODA DO DODAWNIA SLOW
        /// </summary>
        /// <param name="wordDto">KLASA O SŁOWIE</param>
        /// <returns>ZWRACA ID SŁOWA</returns>
        [HttpPost]
        public async Task<ActionResult<int>> AddWord(WordDto wordDto)
        {
            var roleName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value.ToString();
            if(roleName != RoleNames.Admin.ToString())
            {
                return Unauthorized();
            }
            var word = new Word { Name = wordDto.Name, LetterCount = wordDto.Name.Length };

            _context.Set<Word>().Add(word);
            await _context.SaveChangesAsync();

            return Ok(word.Id);
        }
    }
}
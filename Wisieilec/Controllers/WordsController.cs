using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wisieilec.API.Data;
using Wisieilec.Data.Entities;
using Wisieilec.Dtos;

namespace Wisieilec.Controllers
{
    [Route("api/words")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly DataContext _context;

        public WordsController(DataContext context)
        {
            _context = context;
        }

        // POST api/words
        [HttpPost]
        public async Task<IActionResult> AddWord(WordDto wordDto)
        {
            var word = new Word { Name = wordDto.Name, LetterCount = wordDto.Name.Length };

            _context.Set<Word>().Add(word);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
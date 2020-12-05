using System.Collections.Generic;

namespace Wisieilec.Dtos
{
    public class GameStatusDto
    {
        public string Word { get; set; }
        public string UsedLetters { get; set; }
        public int RemainingLives { get; set; }
    }
}
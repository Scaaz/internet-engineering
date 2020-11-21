using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wisieilec.Data.Entities
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public int RemainingLives { get; set; }

        public string UsedLetters { get; set; }

        public int WordId { get; set; }
        public Word Word { get; set; }

        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }
    }
}
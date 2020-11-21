using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wisieilec.Data.Entities
{
    [Table("Lobbies")]
    public class Lobby
    {
        [Key]
        public int Id { get; set; }

        public int LobbySize { get; set; }

        public int NumberOfLives { get; set; }

        public LobbyStatus Status { get; set; }

        public ICollection<User> Users { get; set; }

    }

    public enum LobbyStatus
    {
        New,
        Pending,
        Finished
    }
}
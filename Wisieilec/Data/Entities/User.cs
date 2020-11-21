using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wisieilec.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int TotalScore { get; set; }

        public int? LobbyId { get; set; }
        public Lobby Lobby { get; set; }
    }
}

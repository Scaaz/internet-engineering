using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wisieilec.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }
        public int TotalScore { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int? LobbyId { get; set; }
        public Lobby Lobby { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wisieilec.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8,ErrorMessage = "Password must contain 8 or more characters")]
        public string Password { get; set; }
    }
}

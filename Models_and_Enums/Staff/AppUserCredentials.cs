using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Staff
{
	public class AppUserCredentials
	{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

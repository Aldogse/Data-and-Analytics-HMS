using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Request.Staff
{
	public class RegisterUserCredentialsRequest
	{
        [Required]
        public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password",ErrorMessage = "Password not match")]
		public string ConfirmPassword { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class User
    {
        [Required(ErrorMessage ="First Name is Required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "only alphabet")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "only alphabet")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email ID is Required")]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage="Invalid EmailID")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Name is Required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character from @$!%*#?&")]
        public string Password { get; set; }
        [Required]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "User Name is Required")]
        [Remote("IfUserNameExist", "Account", AdditionalFields = "UserName", ErrorMessage = "Username Exists! Please enter unique name.")]
        public string UserName { get; set; }
        public int IsRegSuccess { get; set; }
        public int IsActive { get; set; }
    }
}

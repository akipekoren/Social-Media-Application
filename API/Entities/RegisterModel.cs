using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class RegisterModel
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string knownAs {get; set;}

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]

        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 4)]
        public string Password {get; set; }


    }
}
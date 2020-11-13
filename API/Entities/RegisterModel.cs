using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class RegisterModel
    {

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Username { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password {get; set; }


    }
}
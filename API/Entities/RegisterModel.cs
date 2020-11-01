using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class RegisterModel
    {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password {get; set; }


    }
}
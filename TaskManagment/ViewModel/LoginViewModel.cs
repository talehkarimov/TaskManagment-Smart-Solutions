using System.ComponentModel.DataAnnotations;

namespace TaskManagment.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is invalid")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is invalid")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

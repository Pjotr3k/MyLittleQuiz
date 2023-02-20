using System.ComponentModel.DataAnnotations;

namespace MyLittleQuiz.ViewModels
{
    public class SignupViewModel
    {
        
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsLoginTaken { get; set; } = false;
        public bool IsEmailTaken { get; set; } = false;

        


    }
}

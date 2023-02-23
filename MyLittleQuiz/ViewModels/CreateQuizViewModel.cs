using System.ComponentModel.DataAnnotations;

namespace MyLittleQuiz.ViewModels
{
    public class CreateQuizViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

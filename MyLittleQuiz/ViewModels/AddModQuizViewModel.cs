using MyLittleQuiz.Models;

namespace MyLittleQuiz.ViewModels
{
    public class AddModQuizViewModel
    {
        public int QuizId { get; set; }
        public List<User> Users { get; set; }
        //public int ModId { get; set; }
        public string Name { get; set; }
    }
}

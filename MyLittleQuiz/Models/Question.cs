namespace MyLittleQuiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; set; }
        public Quiz Quiz { get; set; }

    }
}

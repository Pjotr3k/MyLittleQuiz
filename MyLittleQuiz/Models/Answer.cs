namespace MyLittleQuiz.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        List<ScoreModifier> ScoreModifiers { get; set; }
        public Question Question { get; set; }
    }
}

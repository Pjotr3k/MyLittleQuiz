namespace MyLittleQuiz.Models
{
    public class ScoreModifier
    {
        public int Id { get; set; }
        public Answer Answer { get; set; }
        public ScorePool ScorePool { get; set; }
        public int Modifier { get; set; }

    }
}

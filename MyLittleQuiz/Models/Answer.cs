using MySql.Data.MySqlClient;

namespace MyLittleQuiz.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int AnswerNumber { get; set; }
        public string AnswerText { get; set; }
        Dictionary<ScorePool, int> ScoreModifiers { get; set; }
        public Question Question { get; set; }

        public static List<Answer> PopulateAnswers(int questionId)
        {
            List<Answer> answers = new List<Answer>();

            string sqlQuery = $"SELECT * FROM answers WHERE IdQuestion={questionId}";
            //User user = new User();
            //Quiz quiz = new Quiz();

            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                con.databaseConnection.Close();
                return null;
            }

            while (dr.Read())
            {
                Answer ans = new Answer();


                ans.Id = Convert.ToInt32(dr["IdAnswer"]);
                ans.AnswerNumber = Convert.ToInt32(dr["AnswerNumber"]);
                ans.AnswerText = dr["AnswerText"].ToString();
                //pamiętaj, żeby dać scoremodifers

                ans.Question = Question.GetQuestionById(questionId);

                foreach (var pool in ans.Question.Quiz.ScorePools)
                {
                    int modifier = pool.GetPoolValueForAnswer(ans.Id);
                    ans.ScoreModifiers.Add(pool, modifier);
                }                
            }

            con.databaseConnection.Close();

            return answers;
        }

        public void PopulateScorePools()
        {

        }
    }
}

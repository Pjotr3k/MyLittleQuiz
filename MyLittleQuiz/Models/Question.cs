using MySql.Data.MySqlClient;

namespace MyLittleQuiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; set; }
        public Quiz Quiz { get; set; }

        public static List<Question> PopulateQuizQuestions (int quizId)
        {
            List<Question> questions = new List<Question>();

            string sqlQuery = $"SELECT * FROM questions WHERE IdQuiz={quizId}";
            User user = new User();
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
                Question qu = new Question();


                qu.Id = Convert.ToInt32(dr["IdQuestion"]);
                qu.QuestionNumber = Convert.ToInt32(dr["QuestionNumber"]);
                qu.QuestionText = dr["QuestionText"].ToString();
                qu.Quiz = qu.Quiz.GetQuizById(quizId);

                //here: something to populate answers
            }

            return questions;
        }

        public static Question GetQuestionById(int id)
        {
            Question question = new Question();

            string sqlQuery = $"SELECT * FROM quizzes WHERE IdQuestion='{id}'";
            
            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                con.databaseConnection.Close();
                return null;
            }

            if (dr.Read())
            {
                question.Id= Convert.ToInt32(dr["IdQuestion"]);
                question.QuestionNumber = Convert.ToInt32(dr["IdQuestion"]);
                question.QuestionText = dr["Name"].ToString();
                
                int quizId = Convert.ToInt32(dr["IdQuiz"]);
                question.Quiz = question.Quiz.GetQuizById(quizId);
            }

            con.databaseConnection.Close();

            return question;
        }
    }
}

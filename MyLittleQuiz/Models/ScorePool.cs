using MySql.Data.MySqlClient;

namespace MyLittleQuiz.Models
{
    public class ScorePool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Quiz Quiz { get; set; }

        public static void AddPool(string name, int quizId)
        {
            string sqlQuery = $"INSERT INTO score_pools(Name, IdQuiz) VALUES ('{name}', {quizId})";

            SqlConnection con = new SqlConnection();
            MySqlDataAdapter adp = new MySqlDataAdapter();

            con.databaseConnection.Open();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            adp.InsertCommand = cmd;
            adp.InsertCommand.ExecuteNonQuery();
            con.databaseConnection.Close();
        }

        public static List<ScorePool> GetPoolsByQuiz(Quiz quiz)
        {
            List<ScorePool> pools = new List<ScorePool>();
            int quizId = quiz.Id;

            string sqlQuery = $"SELECT * FROM score_pools WHERE IdQuiz={quizId}";               

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
                ScorePool sp = new ScorePool();

                sp.Id = Convert.ToInt32(dr["IdPool"]);
                sp.Name = dr["Name"].ToString();
                sp.Quiz = quiz;

                pools.Add(sp);
            }

            return pools;
        }
    }
}
